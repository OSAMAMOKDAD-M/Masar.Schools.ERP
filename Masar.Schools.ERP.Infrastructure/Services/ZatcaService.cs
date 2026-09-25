using Masar.Schools.ERP.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Masar.Schools.ERP.Infrastructure.Services;

public class ZatcaService : IZatcaService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ZatcaService> _logger;
    private readonly HttpClient _httpClient;

    public ZatcaService(
        IConfiguration configuration,
        ILogger<ZatcaService> logger,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<string> GenerateInvoiceXmlAsync(Invoice invoice)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            
            // Create UBL 2.1 XML structure for ZATCA Phase 2
            var ublInvoice = xmlDoc.CreateElement("Invoice");
            ublInvoice.SetAttribute("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            ublInvoice.SetAttribute("xmlns:cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ublInvoice.SetAttribute("xmlns:cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            
            xmlDoc.AppendChild(ublInvoice);

            // Invoice ID
            var invoiceId = xmlDoc.CreateElement("cbc:ID");
            invoiceId.InnerText = invoice.InvoiceNumber;
            ublInvoice.AppendChild(invoiceId);

            // Issue Date
            var issueDate = xmlDoc.CreateElement("cbc:IssueDate");
            issueDate.InnerText = invoice.IssueDate.ToString("yyyy-MM-dd");
            ublInvoice.AppendChild(issueDate);

            // Invoice Type Code
            var invoiceType = xmlDoc.CreateElement("cbc:InvoiceTypeCode");
            invoiceType.InnerText = "388"; // Standard invoice type
            ublInvoice.AppendChild(invoiceType);

            // Document Currency Code
            var currency = xmlDoc.CreateElement("cbc:DocumentCurrencyCode");
            currency.InnerText = invoice.Currency;
            ublInvoice.AppendChild(currency);

            // Seller Party (Tenant)
            var sellerParty = xmlDoc.CreateElement("cac:AccountingSupplierParty");
            var sellerPartyName = xmlDoc.CreateElement("cac:Party");
            var sellerName = xmlDoc.CreateElement("cac:PartyName");
            var sellerNameValue = xmlDoc.CreateElement("cbc:Name");
            sellerNameValue.InnerText = invoice.Tenant.NameArabic;
            sellerName.AppendChild(sellerNameValue);
            sellerPartyName.AppendChild(sellerName);
            sellerParty.AppendChild(sellerPartyName);
            ublInvoice.AppendChild(sellerParty);

            // Buyer Party (Student/Guardian)
            var buyerParty = xmlDoc.CreateElement("cac:AccountingCustomerParty");
            var buyerPartyName = xmlDoc.CreateElement("cac:Party");
            var buyerName = xmlDoc.CreateElement("cac:PartyName");
            var buyerNameValue = xmlDoc.CreateElement("cbc:Name");
            buyerNameValue.InnerText = invoice.Student.FullNameArabic;
            buyerName.AppendChild(buyerNameValue);
            buyerPartyName.AppendChild(buyerName);
            buyerParty.AppendChild(buyerPartyName);
            ublInvoice.AppendChild(buyerParty);

            // Invoice Lines
            foreach (var item in invoice.Items)
            {
                var line = xmlDoc.CreateElement("cac:InvoiceLine");
                
                var lineId = xmlDoc.CreateElement("cbc:ID");
                lineId.InnerText = item.Id.ToString();
                line.AppendChild(lineId);

                var quantity = xmlDoc.CreateElement("cbc:InvoicedQuantity");
                quantity.SetAttribute("unitCode", "PCE");
                quantity.InnerText = item.Quantity.ToString();
                line.AppendChild(quantity);

                var lineExtension = xmlDoc.CreateElement("cbc:LineExtensionAmount");
                lineExtension.SetAttribute("currencyID", invoice.Currency);
                lineExtension.InnerText = item.LineTotal.ToString("F2");
                line.AppendChild(lineExtension);

                var itemNode = xmlDoc.CreateElement("cac:Item");
                var itemName = xmlDoc.CreateElement("cbc:Name");
                itemName.InnerText = item.DescriptionArabic;
                itemNode.AppendChild(itemName);
                
                if (!string.IsNullOrEmpty(item.ZatcaItemCode))
                {
                    var sellersItemIdentification = xmlDoc.CreateElement("cac:SellersItemIdentification");
                    var itemId = xmlDoc.CreateElement("cbc:ID");
                    itemId.InnerText = item.ZatcaItemCode;
                    sellersItemIdentification.AppendChild(itemId);
                    itemNode.AppendChild(sellersItemIdentification);
                }
                
                line.AppendChild(itemNode);

                var price = xmlDoc.CreateElement("cac:Price");
                var priceAmount = xmlDoc.CreateElement("cbc:PriceAmount");
                priceAmount.SetAttribute("currencyID", invoice.Currency);
                priceAmount.InnerText = item.UnitPrice.ToString("F2");
                price.AppendChild(priceAmount);
                line.AppendChild(price);

                ublInvoice.AppendChild(line);
            }

            // Tax Total
            var taxTotal = xmlDoc.CreateElement("cac:TaxTotal");
            var taxAmount = xmlDoc.CreateElement("cbc:TaxAmount");
            taxAmount.SetAttribute("currencyID", invoice.Currency);
            taxAmount.InnerText = invoice.TaxAmount.ToString("F2");
            taxTotal.AppendChild(taxAmount);
            ublInvoice.AppendChild(taxTotal);

            // Legal Monetary Total
            var legalTotal = xmlDoc.CreateElement("cac:LegalMonetaryTotal");
            
            var lineExtensionTotal = xmlDoc.CreateElement("cbc:LineExtensionAmount");
            lineExtensionTotal.SetAttribute("currencyID", invoice.Currency);
            lineExtensionTotal.InnerText = invoice.Subtotal.ToString("F2");
            legalTotal.AppendChild(lineExtensionTotal);

            var taxExclusiveTotal = xmlDoc.CreateElement("cbc:TaxExclusiveAmount");
            taxExclusiveTotal.SetAttribute("currencyID", invoice.Currency);
            taxExclusiveTotal.InnerText = invoice.Subtotal.ToString("F2");
            legalTotal.AppendChild(taxExclusiveTotal);

            var taxInclusiveTotal = xmlDoc.CreateElement("cbc:TaxInclusiveAmount");
            taxInclusiveTotal.SetAttribute("currencyID", invoice.Currency);
            taxInclusiveTotal.InnerText = invoice.TotalAmount.ToString("F2");
            legalTotal.AppendChild(taxInclusiveTotal);

            var payableAmount = xmlDoc.CreateElement("cbc:PayableAmount");
            payableAmount.SetAttribute("currencyID", invoice.Currency);
            payableAmount.InnerText = invoice.TotalAmount.ToString("F2");
            legalTotal.AppendChild(payableAmount);

            ublInvoice.AppendChild(legalTotal);

            // Generate hash and sign
            var xmlString = xmlDoc.OuterXml;
            var hash = ComputeSha256Hash(xmlString);
            var signature = SignXmlWithEcDsa(xmlString);

            // Add signature to XML
            var signatureNode = xmlDoc.CreateElement("cac:UBLDocumentSignatures");
            var signatureInfo = xmlDoc.CreateElement("cac:SignatureInformation");
            var signatureId = xmlDoc.CreateElement("cbc:ID");
            signatureId.InnerText = "urn:oasis:names:specification:ubl:signature:1";
            signatureInfo.AppendChild(signatureId);
            
            var signatureMethod = xmlDoc.CreateElement("cbc:SignatureMethod");
            signatureMethod.InnerText = "urn:oasis:names:specification:ubl:signing:1";
            signatureInfo.AppendChild(signatureMethod);
            
            signatureNode.AppendChild(signatureInfo);
            ublInvoice.AppendChild(signatureNode);

            _logger.LogInformation("Generated ZATCA invoice XML for invoice {InvoiceNumber}", invoice.InvoiceNumber);
            return xmlString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating ZATCA invoice XML for invoice {InvoiceNumber}", invoice.InvoiceNumber);
            throw;
        }
    }

    public async Task<string> SubmitInvoiceToZatcaAsync(string invoiceXml, string invoiceNumber)
    {
        try
        {
            var apiUrl = _configuration["ZATCA:ApiUrl"];
            var environment = _configuration["ZATCA:Environment"];

            var endpoint = environment == "Production" 
                ? $"{apiUrl}/invoices/clearance/single" 
                : $"{apiUrl}/invoices/reporting/single";

            var content = new StringContent(invoiceXml, Encoding.UTF8, "application/xml");

            var response = await _httpClient.PostAsync(endpoint, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Successfully submitted invoice {InvoiceNumber} to ZATCA", invoiceNumber);
                return responseContent;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to submit invoice {InvoiceNumber} to ZATCA. Status: {StatusCode}, Error: {Error}", 
                    invoiceNumber, response.StatusCode, errorContent);
                throw new Exception($"ZATCA submission failed: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting invoice {InvoiceNumber} to ZATCA", invoiceNumber);
            throw;
        }
    }

    public string GenerateQrCode(string invoiceXml, string invoiceHash)
    {
        try
        {
            // Create TLV structure for ZATCA QR code
            var tlvData = new StringBuilder();
            
            // Seller Name (Tag 1)
            var sellerName = ExtractSellerName(invoiceXml);
            tlvData.Append("01").Append(sellerName.Length.ToString("X2")).Append(sellerName);
            
            // VAT Number (Tag 2)
            var vatNumber = ExtractVatNumber(invoiceXml);
            tlvData.Append("02").Append(vatNumber.Length.ToString("X2")).Append(vatNumber);
            
            // Invoice Date (Tag 3)
            var invoiceDate = DateTime.Now.ToString("yyyy-MM-dd");
            tlvData.Append("03").Append(invoiceDate.Length.ToString("X2")).Append(invoiceDate);
            
            // Invoice Total (Tag 4)
            var invoiceTotal = ExtractInvoiceTotal(invoiceXml);
            tlvData.Append("04").Append(invoiceTotal.Length.ToString("X2")).Append(invoiceTotal);
            
            // VAT Total (Tag 5)
            var vatTotal = ExtractVatTotal(invoiceXml);
            tlvData.Append("05").Append(vatTotal.Length.ToString("X2")).Append(vatTotal);

            var tlvString = tlvData.ToString();
            var tlvBytes = Encoding.UTF8.GetBytes(tlvString);
            var base64Qr = Convert.ToBase64String(tlvBytes);

            _logger.LogInformation("Generated ZATCA QR code for invoice");
            return base64Qr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating ZATCA QR code");
            throw;
        }
    }

    private string ComputeSha256Hash(string rawData)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private string SignXmlWithEcDsa(string xmlData)
    {
        try
        {
            var certificatePath = _configuration["ZATCA:CertificatePath"];
            var secretKey = _configuration["ZATCA:SecretKey"];

            // In production, load the actual certificate and private key
            // This is a simplified implementation
            var privateKey = GetPrivateKeyFromCertificate(certificatePath, secretKey);
            
            var dataToSign = Encoding.UTF8.GetBytes(xmlData);
            var signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(dataToSign, 0, dataToSign.Length);
            var signature = signer.GenerateSignature();

            return Convert.ToBase64String(signature);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error signing XML with ECDSA");
            throw;
        }
    }

    private AsymmetricKeyParameter GetPrivateKeyFromCertificate(string certificatePath, string secretKey)
    {
        // In production, load the actual private key from the certificate
        // This is a placeholder implementation
        _logger.LogWarning("Using placeholder private key - implement actual certificate loading in production");
        
        // Generate a temporary ECDSA key for demonstration
        var keyPairGenerator = new ECKeyPairGenerator();
        var ecParams = SecNamedCurves.GetByName("secp256k1");
        var keyGenParam = new ECKeyGenerationParameters(
            new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H),
            new SecureRandom()
        );
        keyPairGenerator.Init(keyGenParam);
        var keyPair = keyPairGenerator.GenerateKeyPair();
        
        return keyPair.Private;
    }

    private string ExtractSellerName(string xml)
    {
        // Extract seller name from XML
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var sellerNameNode = xmlDoc.SelectSingleNode("//cac:AccountingSupplierParty//cbc:Name");
        return sellerNameNode?.InnerText ?? "مَسَار للمدارس";
    }

    private string ExtractVatNumber(string xml)
    {
        // Extract VAT number from XML or tenant configuration
        return "300000000000003"; // Placeholder VAT number
    }

    private string ExtractInvoiceTotal(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var totalNode = xmlDoc.SelectSingleNode("//cbc:PayableAmount");
        return totalNode?.InnerText ?? "0.00";
    }

    private string ExtractVatTotal(string xml)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var vatNode = xmlDoc.SelectSingleNode("//cac:TaxTotal//cbc:TaxAmount");
        return vatNode?.InnerText ?? "0.00";
    }
}

public interface IZatcaService
{
    Task<string> GenerateInvoiceXmlAsync(Invoice invoice);
    Task<string> SubmitInvoiceToZatcaAsync(string invoiceXml, string invoiceNumber);
    string GenerateQrCode(string invoiceXml, string invoiceHash);
}
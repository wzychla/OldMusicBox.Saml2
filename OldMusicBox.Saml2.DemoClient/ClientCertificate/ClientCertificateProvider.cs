using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;

namespace OldMusicBox.Saml2.DemoClient
{
    /// <summary>
    /// X509 certificate provider for the client.
    /// Client uses the cert to sign SAML2 requests 
    /// sent to the server
    /// </summary>
    public class ClientCertificateProvider
    {
        private static X509Certificate2 _clientCertificate;

        public X509Certificate2 GetClientCertificate()
        {
            if (_clientCertificate == null )
            {
                var path = HostingEnvironment.MapPath("~/ClientCertificate/SAML2DemoClient.p12");
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    _clientCertificate = new X509Certificate2(bytes, "SAML2", X509KeyStorageFlags.Exportable);
                }
            }

            return _clientCertificate;
        }

        private static X509Certificate2 _idpCertificate;

        public X509Certificate2 GetIdPCertificate()
        {
            if (_idpCertificate == null)
            {
                var path = HostingEnvironment.MapPath("~/ClientCertificate/IDP.cer");
                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    _idpCertificate = new X509Certificate2(bytes);
                }
            }

            return _idpCertificate;
        }
    }
}
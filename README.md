# OldMusicBox.Saml2

The goal of this project is to provide an independent .NET Saml2 Client/Server Library. The implementation follows the 
[official specification](http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf).

Features:

* target classic .NET Framework, make it .NET Core compatible soon
* provide both client and server side implementation of Saml2

Current Version: 0.29

Version History:

* 0.29 - added most of `Response` models
* 0.25 - REDIRECT binding client correctly redirects to the ADFS 
* 0.20 - complete `AuthnRequest` model
* 0.10 - core SAML2 elements: the module and the token 

Roadmap:

* 0.1-0.49 - development versions lacking core features

* 0.5 - first important milestone, client can authenticate agaist Microsoft ADFS using 
    * REDIRECT Request binding
    * REDIRECT Response binding

* 1.0 
    * client side interface so that it's possible to login against ADFS using any combination of supported Request/Response bindings
    * support at least REDIRECT, POST and ARTIFACT client's bindings
    * fit existing `System.IdentityModel` infrastructure, including `IssuerNameRegistry` and `X509CerfificateValidator`

* later on

    * client/server metadata
    * server side next so that it's possible to create the Saml2 Identity Provider compatible with existing clients (e.g. JIRA)
    * support at least REDIRECT and POST server's bindings
    * Assertion encryption

# OldMusicBox.Saml2

The goal of this project is to provide an independent .NET Saml2 Client/Server Library.

Features:

* target classic .NET Framework, make it .NET Core compatible soon
* provide both client and server side implementation of Saml2

Current Version: 0.2

Version History:

* 0.2 - complete `AuthnRequest` model
* 0.1 - core SAML2 elements: the module and the token 

Roadmap:

* 0.1-0.99 - development versions lacking core features

* 1.0 
    * client side interface so that it's possible to login against an example Saml2 Provider (ADFS)
    * support at least REDIRECT, POST and ARTIFACT client's bindings
    * fit existing `System.IdentityModel` infrastructure, including `IssuerNameRegistry` and `X509CerfificateValidator`

* later on

    * server side next so that it's possible to create the Saml2 Identity Provider compatible with existing clients (e.g. JIRA)
    * support at least REDIRECT and POST server's bindings

# OldMusicBox.Saml2

The goal of this project is to provide an independent .NET Saml2 Client/Server Library.

Features:

* target classic .NET Framework, make it .NET Core compatible soon
* provide both client and server side implementation of Saml2

Roadmap:

* client side first so that it's possible to login against an example Saml2 Provider (ADFS)
* support at least REDIRECT, POST and ARTIFACT bindings
* fit existing `System.IdentityModel` infrastructure, including `IssuerNameRegistry` and `X509CerfificateValidator`

* server side next so that it's possible to create the Saml2 Identity Provider compatible with existing clients (e.g. JIRA)
* support at least REDIRECT and POST bindings

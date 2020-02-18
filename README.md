# OldMusicBox.Saml2

The goal of this project is to provide an independent .NET Saml2 Client/Server Library. The implementation follows the 
[official specification](http://docs.oasis-open.org/security/saml/v2.0/saml-core-2.0-os.pdf).

## Current Version: 0.66

Please refer to the change list and the road map below.

## Features:

|  Feature  | Status |
|----|:---:|
|NuGet|yes|
|Unsigned Client REDIRECT binding|yes|
|Signed Client REDIRECT binding|not yet|
|Unsigned Client POST binding|yes|
|Signed Client POST binding|yes|
|Signed Client ARTIFACT binding|yes|
|Client LogoutRequest|yes|
|Client LogoutResponse|yes|
|Server APIs|not yet|
|RSASHA1 signatures|yes|
|RSASHA256 signatures|yes|
|ECDSA signatures|not yet|
|Assertion encryption|not yet|
|Client metadata|not yet|
|Server metadata|not yet|
|.NET Framework|4.6.2+|
|.NET Core|not yet|

## Client interface example

A simplest use case that assumes the POST request/response binding is used:

```C#
public ActionResult Logon()
{
    var saml2    = new Saml2AuthenticationModule();

    // parameters
    var assertionConsumerServiceURL = "https://localhost:44307/account/logon";
    var assertionIssuer             = "https://localhost:44307";
    var identityProvider            = "https://adfs.my.company/adfs/ls/";

    var requestBinding  = Binding.POST;
    var responseBinding = Binding.POST;

    // check if this is 
    if (!saml2.IsSignInResponse(this.Request))
    {
        // AuthnRequest factory
        var authnRequestFactory = new AuthnRequestFactory();

        authnRequestFactory.AssertionConsumerServiceURL = assertionConsumerServiceURL;
        authnRequestFactory.AssertionIssuer             = assertionIssuer;
        authnRequestFactory.Destination                 = identityProvider;

        authnRequestFactory.RequestBinding  = requestBinding;
        authnRequestFactory.ResponseBinding = responseBinding;

        // other options are available for other bindings
        return Content(authnRequestFactory.CreatePostBindingContent());
    }
    else
    {
        // other options are available for other bindings
        var securityToken = saml2.GetPostSecurityToken(this.Request);

        // fail if there is no token
        if ( securityToken == null )
        {
            throw new ArgumentNullException("No security token found in the response accoding to the Response Binding configuration");
        }

        // the token will be validated
        var configuration = new SecurityTokenHandlerConfiguration
        {
            CertificateValidator = X509CertificateValidator.None,
            IssuerNameRegistry   = new DemoClientIssuerNameRegistry(),
            DetectReplayedTokens = false                    
        };
        configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;

        var tokenHandler = new Saml2SecurityTokenHandler()
        {
            Configuration = configuration                    
        };
        var identity     = tokenHandler.ValidateToken(securityToken);

        // the token is validated succesfully
        var principal = new ClaimsPrincipal(identity);
        if (principal.Identity.IsAuthenticated)
        {
            FormsAuthentication.RedirectFromLoginPage(principal.Identity.Name, false);
        }
        else
        {
            throw new ArgumentNullException("principal", "Unauthenticated principal returned from token validation");
        }

        return new EmptyResult();
    }
}
```

## Version History:

* 0.66.0

	- fixed the issue with SAMLArt passed in POST params

* 0.65

    - `LogoutResponse`. Note that there's still some work to do. This is because a SAML2 provider won't correctly terminate the session if the `LogoutRequest` NameID **doesn't exactly match** the NameID
    passed in the Assertion in `AuthnResponse`. However, for this to work:
        - the provider has to issue the Name Identifier claim (which is not default for some providers like ADFS)
        - all NameID parameters (including the `Format`, `NameQualifier` and `SPNameQualifier` have to be stored in the local session so that they can be included in the `LogoutRequest`)

* 0.61

    - `LogoutRequest` POST binding

* 0.60

    - ARTIFACT response binding is supported (that includes the
    `ArtifactResolve`/`ArtifactResponse` handling)

* 0.53

    - `AuthnRequest` is correctly signable, assuming the signing 
    certificate is provided

* 0.51

	- partial work on request signing. This ultimately leads to the ARTIFACT response binding where the `ArtifactResolve` has to be signed.

* 0.51

    - started working on the ARTIFACT response binding

* 0.50

    - first milestone reached. The client can succesfully authenticate
    using POST/REDIRECT request binding and POST response binding

* 0.41

    - Token's signature is validated
    - Signature's certificate is validated against the Issuer name registry

* 0.40

    - Claims identity is already created from the token but the token validation is not yet complete    

* 0.31 
    - SAML response deserializes 

* 0.30 
    - POST binding client correctly redirects to the ADFS    
    - both POST and REDIRECT clients correctly get the response from the ADFS, however the response is not yet parsed and validated as a SAML2 token
* 0.29 
    - added most of `Response` models
* 0.25 
    - REDIRECT binding client correctly redirects to the ADFS 
* 0.20 
    - complete `AuthnRequest` model
* 0.10 
    - core SAML2 elements: the module and the token 

## Roadmap

* 0.75 
    - support REDIRECT response binding
    - support AuthnRequest signing

* 1.0 
    * client side interface so that it's possible to login against ADFS using any combination of supported Request/Response bindings
    * support at least REDIRECT, POST and ARTIFACT client's bindings

* later on

    * support ECDSA certificates (by switching to BouncyCastle)
    * client/server metadata
    * server side next so that it's possible to create the Saml2 Identity Provider compatible with existing clients (e.g. JIRA)
    * support at least REDIRECT and POST server's bindings
    * Assertion encryption
    * support .Net Core

using OldMusicBox.Saml2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldMusicBox.Saml2.Model.Artifact
{
    /// <summary>
    /// Artifact resolve factory
    /// </summary>
    public class ArtifactResolveFactory
    {
        public ArtifactResolve CreateArtifactResolve( string artifact )
        {
            if ( string.IsNullOrEmpty(artifact))
            {
                throw new ArgumentNullException("artifact");
            }

            var artifactResolve = new ArtifactResolve();

            artifactResolve.ID           = string.Format("id_{0}", Guid.NewGuid());
            artifactResolve.Artifact     = artifact;
            artifactResolve.IssueInstant = DateTime.UtcNow;
            artifactResolve.Version      = ProtocolVersion._20;

            return artifactResolve;
        }


    }
}

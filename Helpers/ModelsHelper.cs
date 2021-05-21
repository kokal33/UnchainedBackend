
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Helpers
{
    public static class ModelsHelper
    {
        public static Track MapModelToTrack(TrackModel model) {
           return new Track()
            {
                Description = model.Description,
                OwnerOfPublicAddress = model.OwnerOfPublicAddress,
                Title = model.Title
            };
        }
    }
}

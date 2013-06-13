using System;
using LeanKit.ReleaseManager.Models.Releases;

namespace LeanKit.ReleaseManager.Models
{
    public interface IParsePlannedReleaseDate
    {
        DateTime ParsePlannedDate(ReleaseInputModel release);
    }
}
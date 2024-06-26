﻿using System.Globalization;
using Domain.Properties;
using Domainify.Domain;

namespace Domain.ProjectSettingAggregation
{
    internal class TheProjectHasSomeSprintsFault : InvariantFault
    {
        public TheProjectHasSomeSprintsFault(
            string description = "") :
            base(outerDescription: description,
                innerDescription: string.Format(CultureInfo.CurrentCulture,
                Resource.Invariant_TheProjectHasSomeSprintsFault))
        {
        }
    }
}

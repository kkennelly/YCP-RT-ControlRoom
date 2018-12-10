﻿using System.Collections.Generic;

//
// Changes made here that need to be reflected in the UML (if agreed upon):
//  - SpectraCyberModeType was renamed to SpectraCyberModeTypeEnum
//  - SpectraCyberModeTypeEnum has Unknown instead of Unspecified
//  - SpectraCyberCommandTypeEnum, SpectraCyberRequest, and SpectraCyberResponse were added
//  - BringUnitOnline and BringUnitOffline are now of return type bool
//  - Add all of the attributes used for handling threading
//  - BringUnitOnline and BringUnitOffline were changed to BringUp and BringDown, respectively
//

namespace ControlRoomApplication.Entities
{
    public abstract class AbstractSpectraCyber
    {
        public SpectraCyberModeTypeEnum CurrentModeType { get; set; }
        public SpectraCyberRequest CurrentSpectraCyberRequest { get; set; }

        public AbstractSpectraCyber()
        {
            CurrentModeType = SpectraCyberModeTypeEnum.UNKNOWN;
            CurrentSpectraCyberRequest = SpectraCyberRequest.GetNewEmpty();
        }
    }
}
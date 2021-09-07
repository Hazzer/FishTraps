using VCE_Fishing;

namespace FishTraps
{
    class FishyChecker
    {
        public static bool IsGoodSizeOfFish(FishSizeCategory reference, FishSizeCategory checkedSize)
        {
            switch (reference)
            {
                case FishSizeCategory.Small:
                    return checkedSize == FishSizeCategory.Small;
                case FishSizeCategory.Medium:
                    return checkedSize == FishSizeCategory.Small || checkedSize == FishSizeCategory.Medium;
                case FishSizeCategory.Large:
                    return checkedSize != FishSizeCategory.Special;
                case FishSizeCategory.Special:
                    return false;

            }
            return false;
        }

    }
}

namespace Development.Scripts.Core
{
    public static class GameManager
    {
        public static int carInteractCount = 0;
        public static bool isKeyFound = false;
        public static bool isInTheCar = false;

        public static void VisitCar()
        {
            carInteractCount++;
        }

        public static void FoundKey()
        {
            isKeyFound = true;
        }

        public static void GetInToTheCar()
        {
            isInTheCar = true;
        }
    }
}
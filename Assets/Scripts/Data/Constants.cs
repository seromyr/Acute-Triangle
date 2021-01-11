namespace Constants
{
    public enum GameState
    {
        LOADING,    // Application loads something
        START,      // Application has started and now in MainMenu/Title Screen
        NEW,        // A new game is started
        NEXT,       // A game has won, go to next level
        RUNNING,    // A gameplay is occurring
        PAUSE,      // Gameplay is paused
        WIN,        // A level is victorious, debrief is shown before proceed to the next level
        LOSE,       // A level is lost but can retry
        GAMEOVER    // A level is lost but cannot retry, return to Main Menu/Title Screen
    }

    public enum Direction
    {
        Right,
        Forward
    }

    public static class SceneName
    {
        public const string PRELOAD = "Preload";
        public const string MAINMENU = "MainMenu";
        public const string GAME = "Game";
    }
    public static class Map
    {
        public const string NAME = "Level 0";

        public const string LV000 = "Level 00";
        public const string LV001 = "Level 01";
        public const string LV002 = "Level 02";
        public const string LV003 = "3";
        public const string LV004 = "4";
        public const string LV005 = "5";
        public const string LV006 = "6";
        public const string LV007 = "7";
        public const string LV008 = "8";
        public const string LV009 = "9";
        public const string LV010 = "10";
    }

    public static class PrimeObj
    {
        //public const string GAMEMANAGER = "GameManager";
        public const string PLAYER = "Player";
        //public const string MAINMENU = "MainMenu";
        //public const string INGAMEMENU = "IngameMenu";
    }

    public static class PlayerAttributes
    {
        public const float PLAYER_MAXHEALTH = 20;
        public const float PLAYER_DAMAGE = 1;
    }

    public static class PlayerSkin
    {
        public const string DEFAULT = "PlayerSkin_Default";
        public const string _01 = "PlayerSkin_01";
        public const string _02 = "PlayerSkin_02";
    }

    public static class EnemySkin
    {
        public const string Sphere_Large = "Enemy/Sphere_L";
        public const string Cube_Small = "Enemy/Cube_S";
    }

    public static class EnemyName
    {
        public const string Sphere_Large = "LargeSphere";
        public const string Cube_Small = "SmallCube";
    }
}
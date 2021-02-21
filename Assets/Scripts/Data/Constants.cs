namespace Constants
{
    public static class Version
    {
        public const string NAME = "The Trials of Acute Triangle";
        public const string CURRENTVERSION = "0.2.0.2";
    }

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

    public enum BulletType 
    {
        Destructible,
        Indestructible
    }

    public enum Mechanic
    {
        Shoot,
        AggressiveRadius,
        Fear,
        Retreat,
        HardShells,
        PowerChargers,
        MoveToWaypoints,
        SelfRotation,
        SummonClones,
        SummonMinions,
        Switches,
        Patrol,
        Chase,
        SelfPhase,
        Shield,
        LookAtPlayer
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
        public const string LV003 = "Level 03";
        public const string LV004 = "Level 04";
        public const string LV005 = "Level 05";
        public const string LV006 = "Level 06";
        public const string LV007 = "Level 07";
        public const string LV008 = "Level 08";
        public const string LV009 = "Level 09";
    }

    public static class GeneralConst
    {
        //public const string GAMEMANAGER = "GameManager";
        public const string PLAYER = "Player";
        public const int FLOOR_LAYER = 8;
        public const string BULLET = "Bullet";
        public const float ENEMY_BULLET_SPEED_FAST = 20f;
        public const float ENEMY_BULLET_SPEED_MODERATE = 10f;
        public const float ENEMY_BULLET_SPEED_SLOW = 5f;
        public const int BOSS_00_HEALTH = 50;
        //public const string MAINMENU = "MainMenu";
        //public const string INGAMEMENU = "IngameMenu";
    }

    public static class PlayerAttributes
    {
        public const float PLAYER_MAXHEALTH = 12f;
        public const float PLAYER_DAMAGE = 1f;
        public const float PLAYER_MOVESPEED = 10f;
    }

    public static class PlayerSkin
    {
        public const string DEFAULT = "PlayerSkin_Default";
        public const string _01 = "PlayerSkin_01";
        public const string _02 = "PlayerSkin_02";
    }

    public static class Enemy
    {
        public const string Sphere_Large_Black = "Enemy/Sphere_L_Black";
        public const string Sphere_Medium_Red = "Enemy/Sphere_M_Red";
        public const string Cube_Medium_Black = "Enemy/Cube_M_Black";
        public const string Triangle_Medium_Black = "Enemy/Triangle_Medium_Black";
        public const string PowerPillar = "Enemy/PowerPillar";
    }

    public static class EnemyName
    {
        public const string Sphere_Large = "LargeSphere";
        public const string Cube_Small = "SmallCube";
    }
}
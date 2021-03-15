namespace Constants
{
    public static class Version
    {
        public const string CURRENTVERSION = "0.3.0.3 Pre-Beta";
        public const string BUILD = "1";
        public const string DATE = "03152021";
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
        GAMEOVER,   // A level is lost but cannot retry, return to Main Menu/Title Screen
        LOADLEVEL   // Go to a level specified in Main Menu
    }

    public enum Direction
    {
        Right,
        Forward
    }

    public enum BulletType 
    {
        Destructible,
        Indestructible,
        Mixed,
        Explosive
    }

    public enum Mechanic
    {
        Shoot,
        AggressiveRadius,
        Fear,
        ComplexeMovement,
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
        LookAtPlayer,
    }

    public static class SceneName
    {
        public const string PRELOAD = "Preload";
        public const string MAINMENU = "MainMenu";
        public const string GAME = "Game";
    }
    public static class Map
    {
        public const string NAME = "Level ";

        public const string LV001 = "Level 1";
        public const string LV002 = "Level 2";
        public const string LV003 = "Level 3";
        public const string LV004 = "Level 4";
        public const string LV005 = "Level 5";
        public const string LV006 = "Level 6";
        public const string LV007 = "Level 7";
        public const string LV008 = "Level 8";
        public const string LV009 = "Level 9";
        public const string LV010 = "Level 10";
        public const string LV011 = "Level 11";
        public const string LV012 = "Level 12";
        public const string LV013 = "Level 13";
        public const string LV014 = "Level 14";
        public const string LV015 = "Level 15";
        public const string LV016 = "Level 16";
        public const string LV017 = "Level 17";
        public const string LV018 = "Level 18";
        public const string LV019 = "Level 19";
        public const string LV020 = "Level 20";
    }

    public static class GeneralConst
    {
        //public const string GAMEMANAGER = "GameManager";
        public const string PLAYER = "Player/Player";
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
        public const float PLAYER_MAXHEALTH = 15f;
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
        public const string Boss_01 = "Enemy/Boss_01";
        public const string Boss_02 = "Enemy/Boss_02";

        public const string Sphere_Large_Black = "Enemy/Sphere_L_Black";
        public const string Sphere_Medium_Red = "Enemy/Sphere_M_Red";
        public const string Sphere_Medium_Red_HalfShell = "Enemy/Sphere_M_Red_HalfShell";
        public const string Cube_Medium_Black = "Enemy/Cube_M_Black";
        public const string Triangle_Medium_Black = "Enemy/Triangle_Medium_Black";
        public const string Cylinder_Medium_Black = "Enemy/Cylinder_Medium_Black";
        public const string PowerReactor = "Enemy/PowerPillar";
    }

    public static class EnemyName
    {
        public const string Sphere_Large = "LargeSphere";
        public const string Cube_Small = "SmallCube";
    }

    public static class Bullet
    {
        public const string _01 = "Prefabs/Bullets/Bullet_01";
        public const string _02 = "Prefabs/Bullets/Bullet_02";
        public const string _03 = "Prefabs/Bullets/Bullet_03";
        public const string _04 = "Prefabs/Bullets/Bullet_04";
        public const string _05 = "Prefabs/Bullets/Bullet_05";
        public const string _06 = "Prefabs/Bullets/Bullet_06";
        public const string _07 = "Prefabs/Bullets/Bullet_07";
    }
}
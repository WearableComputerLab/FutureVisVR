using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStudyInterface : MonoBehaviour
{
    [SerializeField]
    int ParticiantID;
    [SerializeField]
    private bool GamePlay = false;
    [SerializeField]
    private bool ShowArrow = false;
    [SerializeField]
    private bool showHeatmap = false;
    [SerializeField]
    private bool ShowMiniature = false;

    public enum GamePlayers
    {
        None,
        BluePlayer0, BluePlayer1, BluePlayer2, BluePlayer3, BluePlayer4, BluePlayer5, BluePlayer6, BluePlayer7, BluePlayer8, BluePlayer9, BluePlayer10,
        RedPlayer0, RedPlayer1, RedPlayer2, RedPlayer3, RedPlayer4, RedPlayer5, RedPlayer6, RedPlayer7, RedPlayer8, RedPlayer9, RedPlayer10

    }

    public GamePlayers HighlightedGamePlayer = GamePlayers.None;
    public GamePlayers AttachedGamePlayer = GamePlayers.None;
    private GamePlayers temp_HighlightedGamePlayer = GamePlayers.None;
    private GamePlayers temp_AttachedGamePlayer = GamePlayers.None;



    private void LateUpdate()
    {
        MovableFootball.gamePlay = GamePlay;
        MovableFootball.showFuture = ShowArrow;
        MovableFootball.showHeatmap = showHeatmap;
        MovableFootball.showMiniatureView = ShowMiniature;
        MovableFootball.showMovableMiniature = ShowMiniature;

        if (temp_HighlightedGamePlayer != HighlightedGamePlayer)
        {
            temp_HighlightedGamePlayer = HighlightedGamePlayer;
            switch (HighlightedGamePlayer)
            {
                case GamePlayers.None:
                    GoldHaloEffect.createHightedPlayer(null);
                    break;
                case GamePlayers.BluePlayer0:
                    GoldHaloEffect.createHightedPlayer("RightPlayer0");
                    break;
                case GamePlayers.BluePlayer1:
                    GoldHaloEffect.createHightedPlayer("RightPlayer1");
                    break;
                case GamePlayers.BluePlayer2:
                    GoldHaloEffect.createHightedPlayer("RightPlayer2");
                    break;
                case GamePlayers.BluePlayer3:
                    GoldHaloEffect.createHightedPlayer("RightPlayer3");
                    break;
                case GamePlayers.BluePlayer4:
                    GoldHaloEffect.createHightedPlayer("RightPlayer4");
                    break;
                case GamePlayers.BluePlayer5:
                    GoldHaloEffect.createHightedPlayer("RightPlayer5");
                    break;
                case GamePlayers.BluePlayer6:
                    GoldHaloEffect.createHightedPlayer("RightPlayer6");
                    break;
                case GamePlayers.BluePlayer7:
                    GoldHaloEffect.createHightedPlayer("RightPlayer7");
                    break;
                case GamePlayers.BluePlayer8:
                    GoldHaloEffect.createHightedPlayer("RightPlayer8");
                    break;
                case GamePlayers.BluePlayer9:
                    GoldHaloEffect.createHightedPlayer("RightPlayer9");
                    break;
                case GamePlayers.BluePlayer10:
                    GoldHaloEffect.createHightedPlayer("RightPlayer10");
                    break;
                case GamePlayers.RedPlayer0:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer0");
                    break;
                case GamePlayers.RedPlayer1:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer1");
                    break;
                case GamePlayers.RedPlayer2:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer2");
                    break;
                case GamePlayers.RedPlayer3:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer3");
                    break;
                case GamePlayers.RedPlayer4:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer4");
                    break;
                case GamePlayers.RedPlayer5:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer5");
                    break;
                case GamePlayers.RedPlayer6:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer6");
                    break;
                case GamePlayers.RedPlayer7:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer7");
                    break;
                case GamePlayers.RedPlayer8:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer8");
                    break;
                case GamePlayers.RedPlayer9:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer9");
                    break;
                case GamePlayers.RedPlayer10:
                    GoldHaloEffect.createHightedPlayer("LeftPlayer10");
                    break;
            }
        }

        if (temp_AttachedGamePlayer != AttachedGamePlayer)
        {
            temp_AttachedGamePlayer = AttachedGamePlayer;
            switch (AttachedGamePlayer)
            {
                case GamePlayers.None:
                    CameraFollow.resetPosition();
                    break;
                case GamePlayers.BluePlayer0:
                    CameraFollow.AttachedGamePlayer = "RightPlayer0";
                    break;
                case GamePlayers.BluePlayer1:
                    CameraFollow.AttachedGamePlayer = "RightPlayer1";
                    break;
                case GamePlayers.BluePlayer2:
                    CameraFollow.AttachedGamePlayer = "RightPlayer2";
                    break;
                case GamePlayers.BluePlayer3:
                    CameraFollow.AttachedGamePlayer = "RightPlayer3";
                    break;
                case GamePlayers.BluePlayer4:
                    CameraFollow.AttachedGamePlayer = "RightPlayer4";
                    break;
                case GamePlayers.BluePlayer5:
                    CameraFollow.AttachedGamePlayer = "RightPlayer5";
                    break;
                case GamePlayers.BluePlayer6:
                    CameraFollow.AttachedGamePlayer = "RightPlayer6";
                    break;
                case GamePlayers.BluePlayer7:
                    CameraFollow.AttachedGamePlayer = "RightPlayer7";
                    break;
                case GamePlayers.BluePlayer8:
                    CameraFollow.AttachedGamePlayer = "RightPlayer8";
                    break;
                case GamePlayers.BluePlayer9:
                    CameraFollow.AttachedGamePlayer = "RightPlayer9";
                    break;
                case GamePlayers.BluePlayer10:
                    CameraFollow.AttachedGamePlayer = "RightPlayer10";
                    break;
                case GamePlayers.RedPlayer0:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer0";
                    break;
                case GamePlayers.RedPlayer1:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer1";
                    break;
                case GamePlayers.RedPlayer2:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer2";
                    break;
                case GamePlayers.RedPlayer3:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer3";
                    break;
                case GamePlayers.RedPlayer4:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer4";
                    break;
                case GamePlayers.RedPlayer5:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer5";
                    break;
                case GamePlayers.RedPlayer6:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer6";
                    break;
                case GamePlayers.RedPlayer7:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer7";
                    break;
                case GamePlayers.RedPlayer8:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer8";
                    break;
                case GamePlayers.RedPlayer9:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer9";
                    break;
                case GamePlayers.RedPlayer10:
                    CameraFollow.AttachedGamePlayer = "LeftPlayer10";
                    break;
            }
        }

    }
}

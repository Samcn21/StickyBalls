using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData
{

    public enum Team
    {
        Purple,
        Blue,
        Yellow,
        Cyan,
        Neutral
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public enum PlayerSourceDirection
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static Dictionary<Team, Color> TeamColors = new Dictionary<Team, Color>() 
        {

            {Team.Cyan,          new Color32(0, 255, 255 , 255) },
            {Team.Purple,        new Color32(255, 0, 255 , 255) },
            {Team.Yellow,        new Color32(255, 215, 0 , 255) },
            {Team.Blue,          new Color32(0, 50, 200 , 255) },
            {Team.Neutral,       new Color32(232, 232, 232 , 255) }
        };

    public static Dictionary<Team, Color> CoopTeamColors = new Dictionary<Team, Color>()
        {
            {Team.Cyan,          new Color32(0, 255, 255 , 255) },
            {Team.Purple,        new Color32(255, 0, 255 , 255) },
            {Team.Neutral,       new Color32(232, 232, 232 , 255) }
        };

    public enum SourceMachineStates
    {
        SourceMachineBlue,
        SourceMachineCyan,
        SourceMachinePurple,
        SourceMachineYellow
    }

    public enum CenterMachineStates
    {
        CenterMachineNeutral,
        CenterMachineBlue,
        CenterMachineCyan,
        CenterMachinePurple,
        CenterMachineYellow
    }

    public enum ControllerStates 
    {
        XboxController,
        XboxLeftScroll,
        XboxA,
        XboxB,
        XboxStart,
        PsController,
        PsLeftScroll,
        PsX,
        PsCircle,
        PsStart
    }

    public enum PipesStates
    {
        PipeNeutralEmptyCorner,
        PipeNeutralEmptyT,
        PipeNeutralEmptyStraight,
        PipeNeutralEmptyCross,

        PipeNeutralAllowedCorner,
        PipeNeutralAllowedT,
        PipeNeutralAllowedStraight,
        PipeNeutralAllowedCross,

        PipeNeutralForbiddenCorner,
        PipeNeutralForbiddenT,
        PipeNeutralForbiddenStraight,
        PipeNeutralForbiddenCross,

        PipeBlueEmptyCorner,
        PipeBlueEmptyT,
        PipeBlueEmptyStraight,
        PipeBlueEmptyCross,

        PipeCyanEmptyCorner,
        PipeCyanEmptyT,
        PipeCyanEmptyStraight,
        PipeCyanEmptyCross,

        PipePurpleEmptyCorner,
        PipePurpleEmptyT,
        PipePurpleEmptyStraight,
        PipePurpleEmptyCross,

        PipeYellowEmptyCorner,
        PipeYellowEmptyT,
        PipeYellowEmptyStraight,
        PipeYellowEmptyCross,

        PipeBlueFullCorner,
        PipeBlueFullT,
        PipeBlueFullStraight,
        PipeBlueFullCross,

        PipeCyanFullCorner,
        PipeCyanFullT,
        PipeCyanFullStraight,
        PipeCyanFullCross,

        PipePurpleFullCorner,
        PipePurpleFullT,
        PipePurpleFullStraight,
        PipePurpleFullCross,

        PipeYellowFullCorner,
        PipeYellowFullT,
        PipeYellowFullStraight,
        PipeYellowFullCross
    }

    public enum PlayerState
    {
        IdleFront,
        MovementFront,
        IdleBack,
        MovementBack,
        IdleRight,
        MovementRight,
        IdleLeft,
        MovementLeft,
        PipeGrabFront,
        PipeGrabBack,
        PipeGrabRight,
        PipeGrabLeft,
        PipePlaceFront,
        PipePlaceBack,
        PipePlaceRight,
        PipePlaceLeft,
        MovementFrontCarryPipe,
        MovementBackCarryPipe,
        MovementRightCarryPipe,
        MovementLeftCarryPipe,
        Dance
    }

    public enum AnimationStates
    {
        //Character States
        IdleFront,
        MovementFront,
        IdleBack,
        MovementBack,
        IdleRight,
        MovementRight,
        IdleLeft,
        MovementLeft,
        PipeGrabFront,
        PipeGrabBack,
        PipeGrabRight,
        PipeGrabLeft,
        PipePlaceFront,
        PipePlaceBack,
        PipePlaceRight,
        PipePlaceLeft,

        //Pipes States
        PipeNeutralEmptyCorner,
        PipeNeutralEmptyT,
        PipeNeutralEmptyStraight,
        PipeNeutralEmptyCross,

        PipeBlueEmptyCorner,
        PipeBlueEmptyT,
        PipeBlueEmptyStraight,
        PipeBlueEmptyCross,

        PipeCyanEmptyCorner,
        PipeCyanEmptyT,
        PipeCyanEmptyStraight,
        PipeCyanEmptyCross,

        PipePurpleEmptyCorner,
        PipePurpleEmptyT,
        PipePurpleEmptyStraight,
        PipePurpleEmptyCross,

        PipeYellowEmptyCorner,
        PipeYellowEmptyT,
        PipeYellowEmptyStraight,
        PipeYellowEmptyCross,

        PipeBlueFullCorner,
        PipeBlueFullT,
        PipeBlueFullStraight,
        PipeBlueFullCross,

        PipeCyanFullCorner,
        PipeCyanFullT,
        PipeCyanFullStraight,
        PipeCyanFullCross,

        PipePurpleFullCorner,
        PipePurpleFullT,
        PipePurpleFullStraight,
        PipePurpleFullCross,

        PipeYellowFullCorner,
        PipeYellowFullT,
        PipeYellowFullStraight,
        PipeYellowFullCross,

        //Sources Machine
        SourceMachineCyan,
        SourceMachineBlue,
        SourceMachinePurple,
        SourceMachineYellow,

        //Center Machine
        CenterMachineNeutral,
        CenterMachineBlue,
        CenterMachineCyan,
        CenterMachinePurple,
        CenterMachineYellow

    }

    public enum SpriteSheet
    {
        Character,
        Pipe,
        PipeNeutral,
        CenterMachine,
        SourceMachine,
        XboxController,
        PsController
    }

    public enum AudioClipState
    {
        Music,
        PickupPipe,
        PlacePipe,
        PushOthers,
        Walking,
        Explosion,
        Winning,
        RotatePipe
    }

    public enum GameStates
    {
        Begin,
        Menu,
        ColorAssignFFA,
        ColorAssign2vs2,
        PlayFFA,
        Play2vs2,
        WinningFFA,
        Winning2vs2,
        Restart,
        Pause,
        Replay  //for ghost trail later on

    }
    public class Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Coordinate c = obj as Coordinate;
            if ((System.Object)c == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (x == c.x) && (y == c.y);
        }

    }


}


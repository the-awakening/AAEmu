﻿using System;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units.Movements;
using AAEmu.Game.Models.Tasks.UnitMove;

namespace AAEmu.Game.Models.Game.Units.Route
{
    /// <summary>
    /// 正圆形巡航路线
    /// 根据圆点进行正圆形路线行走，适合平面地区
    /// 非平整地区会造成NPC的遁地或飞空
    /// </summary>
    public class Square: Patrol
    {
        public static uint Seq = 0;
        public static uint Count = 0;
        public static short VelZ = 0;


        public sbyte Radius { get; set; } = 5;
        public short Degree { get; set; } = 360;

        /// <summary>
        /// 正方形巡航
        /// </summary>
        /// <param name="caster">触发角色</param>
        /// <param name="npc">NPC</param>
        /// <param name="degree">角度 默认360度</param>
        public override void Execute(Unit caster, Npc npc)
        {
            if (Count < Degree / 2)
            {
                npc.Position.X += (float)0.1;
            }
            else if (Count < Degree)
            {
                npc.Position.X -= (float)0.1;
            }

            if (Count < Degree / 4 || (Count > (Degree / 4 + Degree / 2) && Count < Degree))
            {
                npc.Position.Y += (float)0.1;
            }
            else if (Count < (Degree / 4 + Degree / 2))
            {
                npc.Position.Y -= (float)0.1;
            }

            //模拟unit
            var type = (MoveTypeEnum)1;
            //返回moveType对象
            var moveType = (UnitMoveType)MoveType.GetType(type);

            //改变NPC坐标
            moveType.X = npc.Position.X;
            moveType.Y = npc.Position.Y;
            moveType.Z = npc.Position.Z;
            moveType.Flags = 5;
            moveType.VelZ = VelZ;
            moveType.DeltaMovement = new sbyte[3];
            moveType.DeltaMovement[0] = 0;
            moveType.DeltaMovement[1] = 127;
            moveType.DeltaMovement[2] = 0;
            moveType.Stance = 0;
            moveType.Alertness = 2;
            moveType.Time = Seq;


            caster.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);

            if (Count < Degree)
                TaskManager.Instance.Schedule(
                           new UnitMove(this, caster, npc), TimeSpan.FromMilliseconds(100)
                        );
            else
            {
                moveType.DeltaMovement[1] = 0;
                caster.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);
                Count = 0;
            }
        }
    }
}

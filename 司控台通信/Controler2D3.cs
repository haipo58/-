using System;
using System.IO;

namespace 司控台通信
{
    public class Controler2D3
    {
        // 包类型
        public enum PackageType
        {
            None,
            Message // 数据包
        }

        public enum HandlePosition
        {
            None = 0xFF,

            SpeedUp_1 = 0x01,
            SpeedUp_2 = 0x02,
            SpeedUp_3 = 0x03,
            SpeedUp_4 = 0x04,

            SpeedDown_1 = 0x11,
            SpeedDown_2 = 0x12,
            SpeedDown_3 = 0x13,
            SpeedDown_4 = 0x14,
            SpeedDown_5 = 0x15,
            SpeedDown_6 = 0x16,
            SpeedDown_7 = 0x17,
            SpeedDown_8 = 0x18,
        }

        public enum knob3Pos
        {
            Zero = 0xFF, One = 0x05, Two = 0x0A
        }

        public enum ButtonPos
        {
            Down = 0x55, Up = 0xAA
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="buf">数据</param>
        public void Unpack(byte[] buf)
        {
            using (var stream = new MemoryStream(buf))
            using (var reader = new BinaryReader(stream))
            {
                Head = reader.ReadUInt16();
                Cycle = reader.ReadUInt16();
                PackType = (PackageType)reader.ReadByte();
                TrainId = reader.ReadByte();
                HandlePos = (HandlePosition)reader.ReadByte();

                knob3Count = reader.ReadByte();
                DoorStatus = (knob3Pos)reader.ReadByte();
                FrontLight = (knob3Pos)reader.ReadByte();
                ReverseStatus = (knob3Pos)reader.ReadByte();

                ButtonCount = reader.ReadByte();
                遮阳伞 = (ButtonPos)reader.ReadByte();
                水泵 = (ButtonPos)reader.ReadByte();
                电笛 = (ButtonPos)reader.ReadByte();
                紧急制动 = (ButtonPos)reader.ReadByte();
                强缓 = (ButtonPos)reader.ReadByte();
                强制泵风 = (ButtonPos)reader.ReadByte();
                左开门 = (ButtonPos)reader.ReadByte();
                右开门 = (ButtonPos)reader.ReadByte();
                门允许 = (ButtonPos)reader.ReadByte();
                ATO = (ButtonPos)reader.ReadByte();
                模式升 = (ButtonPos)reader.ReadByte();
                模式降 = (ButtonPos)reader.ReadByte();
                确认 = (ButtonPos)reader.ReadByte(); 
                自动折返 = (ButtonPos)reader.ReadByte();
                激活钥匙 = (ButtonPos)reader.ReadByte();
                复位 = (ButtonPos)reader.ReadByte();
                试灯 = (ButtonPos)reader.ReadByte();
                左关门 = (ButtonPos)reader.ReadByte();
                右关门 = (ButtonPos)reader.ReadByte();
            }
        }

        public UInt16 Head { get; set; }
        public UInt16 Cycle { get; set; }
        public PackageType PackType { get; set; }
        public byte TrainId { get; set; }
        public HandlePosition HandlePos { get; set; } = HandlePosition.None;
        
        public byte knob3Count { get; set; }
        public knob3Pos DoorStatus { get; set; } = knob3Pos.Zero;
        public knob3Pos FrontLight { get; set; } = knob3Pos.Zero;
        public knob3Pos ReverseStatus { get; set; } = knob3Pos.Zero;

        public byte ButtonCount { get; set; }
        public ButtonPos 遮阳伞 { get; set; } = ButtonPos.Down;
        public ButtonPos 水泵 { get; set; } = ButtonPos.Up;
        public ButtonPos 电笛 { get; set; } = ButtonPos.Up;
        public ButtonPos 紧急制动 { get; set; } = ButtonPos.Up;
        public ButtonPos 强缓 { get; set; } = ButtonPos.Up;
        public ButtonPos 强制泵风 { get; set; } = ButtonPos.Up;
        public ButtonPos 左开门 { get; set; } = ButtonPos.Up;
        public ButtonPos 右开门 { get; set; } = ButtonPos.Up;
        public ButtonPos 门允许 { get; set; } = ButtonPos.Up;
        public ButtonPos ATO { get; set; } = ButtonPos.Up;
        public ButtonPos 模式升 { get; set; } = ButtonPos.Up;
        public ButtonPos 模式降 { get; set; } = ButtonPos.Up;
        public ButtonPos 确认 { get; set; } = ButtonPos.Up;
        public ButtonPos 自动折返 { get; set; } = ButtonPos.Up;
        public ButtonPos 激活钥匙 { get; set; } = ButtonPos.Up;
        public ButtonPos 复位 { get; set; } = ButtonPos.Up;
        public ButtonPos 试灯 { get; set; } = ButtonPos.Up;
        public ButtonPos 左关门 { get; set; } = ButtonPos.Up;
        public ButtonPos 右关门 { get; set; } = ButtonPos.Up;
    }
}

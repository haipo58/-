using System;
using System.IO;

namespace 司控台通信
{
    public class D3ToControler
    {
        public UInt16 Head { get; set; } = 0x55AA;
        public UInt16 Cycle { get; set; }
        public byte PackType { get; set; } = 1;
        public byte TrainId { get; set; } = 0;
        public byte KnobCount { get; set; } = 2;
        public byte DoorKnob { get; set; }
        public byte ReversKnob { get; set; }
        public byte ButtonCount { get; set; } = 7;
        public bool IsEb { get; set; }
        public bool IsDoorMode { get; set; }
        public bool IsDoorAllowButton { get; set; }
        public bool IsReleaseLightFail { get; set; }
        public bool IsDoorCloseLight { get; set; }
        public bool IsDoorOpenLight { get; set; }
        public bool IsDoorAllow { get; set; }

        public int PackLen { get; private set; }

        private byte[] buf = new byte[1024];
        private Controler2D3.knob3Pos[] KnobPoses = new Controler2D3.knob3Pos[3]
        {
            Controler2D3.knob3Pos.Zero, Controler2D3.knob3Pos.One, Controler2D3.knob3Pos.Two
        };

        internal byte[] Pack()
        {
            using (MemoryStream mStream = new MemoryStream(buf))
            using(BinaryWriter bWriter = new BinaryWriter(mStream))
            {
                bWriter.Write(Head);
                bWriter.Write(Cycle++);
                bWriter.Write(PackType);
                bWriter.Write(TrainId);
                bWriter.Write(KnobCount);
                bWriter.Write((byte)KnobPoses[DoorKnob]);
                bWriter.Write((byte)KnobPoses[ReversKnob]);
                bWriter.Write(ButtonCount);
                bWriter.Write((byte)(IsEb ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsDoorMode ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsDoorAllowButton ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsReleaseLightFail ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsDoorCloseLight ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsDoorOpenLight ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));
                bWriter.Write((byte)(IsDoorAllow ? Controler2D3.ButtonPos.Down : Controler2D3.ButtonPos.Up));

                PackLen = (int)mStream.Position;
            }

            return buf;
        }
    }
}

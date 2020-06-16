
//Warning: all code of this file are generated automatically, so do not modify it manually ~

using UnityEngine;
using System;
using System.Collections.Generic;
using Core.IO;

namespace Metadata
{
    [Serializable]
    sealed partial class MusicData : ILoadable
#if UNITY_EDITOR
    , ISavable
#endif
    {
#if UNITY_EDITOR
        [Export(ExportFlags.AutoCode)]
        public override void Save (IOctetsWriter writer)
        {
            writer.Write(backGroundAndio);
            writer.Write(clickBtn);
            writer.Write(login);
            writer.Write(readyMusic);
            writer.Write(goMusic);
            writer.Write(gameWin);
            writer.Write(gameLose);
            writer.Write(throwCraps);
            writer.Write(move);
            writer.Write(bgMusic1);
            writer.Write(bgMusic2);
            writer.Write(bgMusic3);
            writer.Write(slectRole);
            writer.Write(startBgSound);
            writer.Write(bgNewAdd1);
            writer.Write(bgNewAdd2);
            writer.Write(bgNewAdd3);
            writer.Write(getBaby);
        }
#endif
        [Export(ExportFlags.AutoCode)]
        public override void Load (IOctetsReader reader)
        {
            backGroundAndio = reader.ReadString();
            clickBtn = reader.ReadString();
            login = reader.ReadString();
            readyMusic = reader.ReadString();
            goMusic = reader.ReadString();
            gameWin = reader.ReadString();
            gameLose = reader.ReadString();
            throwCraps = reader.ReadString();
            move = reader.ReadString();
            bgMusic1 = reader.ReadString();
            bgMusic2 = reader.ReadString();
            bgMusic3 = reader.ReadString();
            slectRole = reader.ReadString();
            startBgSound = reader.ReadString();
            bgNewAdd1 = reader.ReadString();
            bgNewAdd2 = reader.ReadString();
            bgNewAdd3 = reader.ReadString();
            getBaby = reader.ReadString();
        }

        public override string ToString ()
        {
            return string.Format("[MusicData:ToString()] backGroundAndio={0}, clickBtn={1}, login={2}, readyMusic={3}, goMusic={4}, gameWin={5}, gameLose={6}, throwCraps={7}, move={8}, bgMusic1={9}, bgMusic2={10}, bgMusic3={11}, slectRole={12}, startBgSound={13}, bgNewAdd1={14}, bgNewAdd2={15}, bgNewAdd3={16}, getBaby={17}", backGroundAndio, clickBtn, login, readyMusic, goMusic, gameWin, gameLose, throwCraps, move, bgMusic1, bgMusic2, bgMusic3, slectRole, startBgSound, bgNewAdd1, bgNewAdd2, bgNewAdd3, getBaby);
        }
        [Export(ExportFlags.AutoCode)]
        public override ushort GetMetadataType ()
        {
            throw new NotImplementedException("This method should be override~");
        }

    }

}
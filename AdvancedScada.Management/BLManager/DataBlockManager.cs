﻿using System;
using System.Collections.Generic;
using System.Xml;
using AdvancedScada.DriverBase.Devices;

namespace AdvancedScada.Management.BLManager
{

    public class DataBlockManager
    {
        public const string DATABLOCK = "DataBlock";
        public const string CHANNEL_ID = "ChannelId";
        public const string DEVICE_ID = "DeviceId";
        public const string DATABLOCK_ID = "DataBlockId";
        public const string DATABLOCK_NAME = "DataBlockName";
        public const string TypeOfRead = "TypeOfRead";
        public const string START_ADDRESS = "StartAddress";
        public const string MemoryType = "MemoryType";
        public const string LENGTH = "Length";
        public const string DATA_TYPE = "DataType";
        private static readonly object mutex = new object();
        private static DataBlockManager _instance;

        private readonly TagManagerXML objTagManager;
        public DataBlockManager()
        {
            objTagManager = new TagManagerXML();
        }

        public static DataBlockManager GetDataBlockManager()
        {
            lock (mutex)
            {
                if (_instance == null) _instance = new DataBlockManager();
            }

            return _instance;
        }
        /// <summary>
        ///     Thêm mới gói dữ liệu.
        /// </summary>
        /// <param name="dv">gói dữ liệu</param>
        /// <param name="dv">gói dữ liệu</param>
        public void Add(Device dv, DataBlock db)
        {
            try
            {
                if (db == null) throw new NullReferenceException("The DataBlock is null reference exception");
                var fDv = IsExisted(dv, db);
                if (fDv != null)
                    throw new Exception($"DataBlock name: '{db.DataBlockName}' is existed");
                dv.DataBlocks.Add(db);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Add(Channel ch, Device dv, DataBlock db)
        {
            Device result = null;
            try
            {
                if (db == null) throw new NullReferenceException("The DataBlock is null reference exception");
                foreach (var item in ch.Devices)
                    if (item.DeviceId == dv.DeviceId && item.DeviceName.Equals(dv.DeviceName))
                    {
                        result = item;
                        break;
                    }

                var fDv = IsExisted(result, db);
                if (fDv != null)
                    throw new Exception($"DataBlock name: '{db.DataBlockName}' is existed");
                result.DataBlocks.Add(db);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///     Cập nhật gói dữ liệu.
        /// </summary>
        /// <param name="dv">Thiết bị</param>
        /// <param name="db">gói dữ liệu</param>
        public void Update(Device dv, DataBlock db)
        {
            try
            {
                if (db == null) throw new NullReferenceException("The DataBlock is null reference exception");
                var fCh = IsExisted(dv, db);
                if (fCh != null)
                    throw new Exception($"DataBlock name: '{db.DataBlockName}' is existed");
                foreach (var item in dv.DataBlocks)
                    if (item.DataBlockId == db.DataBlockId)
                    {
                        item.ChannelId = db.ChannelId;
                        item.DeviceId = db.DeviceId;
                        item.DataBlockId = db.DataBlockId;
                        item.DataBlockName = db.DataBlockName;
                        item.Description = db.Description;
                        item.TypeOfRead = db.TypeOfRead;
                        item.StartAddress = db.StartAddress;
                        item.MemoryType = db.MemoryType;
                        item.Length = db.Length;
                        break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Xóa gói dữ liệu.
        /// </summary>
        /// <param name="dv">Thiết bị</param>
        /// <param name="dbId">Mã gói dữ liệu</param>
        public void Delete(Device dv, int dbId)
        {
            try
            {
                var result = GetByDataBlockId(dv, dbId);
                if (result == null) throw new KeyNotFoundException("DataBlock Id is not found exception");
                dv.DataBlocks.Remove(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Xóa gói dữ liệu.
        /// </summary>
        /// <param name="dv">Thiết bị</param>
        /// <param name="dbName">Tên gói dữ liệu</param>
        public void Delete(Device dv, string dbName)
        {
            try
            {
                var result = GetByDataBlockName(dv, dbName);
                if (result == null) throw new KeyNotFoundException("DataBlock name is not found exception");
                dv.DataBlocks.Remove(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Xóa gói dữ liệu.
        /// </summary>
        /// <param name="dv">Thiết bị</param>
        /// <param name="db">gói dữ liệu</param>
        public void Delete(Device dv, DataBlock db)
        {
            try
            {
                if (db == null) throw new NullReferenceException("The DataBlock is null reference exception");
                foreach (var item in dv.DataBlocks)
                    if (item.DataBlockId == db.DataBlockId)
                    {
                        dv.DataBlocks.Remove(item);
                        break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Phương thức kiểm tra gói dữ liệu đã tồn tại chưa?
        /// </summary>
        /// <param name="dv">Thiết bị</param>
        /// <param name="db">gói dữ liệu</param>
        /// <returns>gói dữ liệu</returns>
        public DataBlock IsExisted(Device dv, DataBlock db)
        {
            DataBlock result = null;
            try
            {
                foreach (var item in dv.DataBlocks)
                    if (item.DataBlockId != db.DataBlockId && item.DataBlockName.Equals(db.DataBlockName))
                    {
                        result = item;
                        break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        ///     Tìm kiếm gói dữ liệu theo mã gói dữ liệu.
        /// </summary>
        /// <param name="ch">Thiết bị</param>
        /// <param name="chId">Mã gói dữ liệu</param>
        /// <returns>Gói dữ liệu</returns>
        public DataBlock GetByDataBlockId(Device ch, int chId)
        {
            DataBlock result = null;
            try
            {
                foreach (var item in ch.DataBlocks)
                    if (item.DataBlockId == chId)
                    {
                        result = item;
                        break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        ///     Tìm kiếm gói dữ liệu theo tên gói dữ liệu.
        /// </summary>
        /// <param name="ch">Thiết bị</param>
        /// <param name="chName">Tên gói dữ liệu</param>
        /// <returns>Gói dữ liệu</returns>
        public DataBlock GetByDataBlockName(Device ch, string chName)
        {
            DataBlock result = null;
            try
            {
                foreach (var item in ch.DataBlocks)
                    if (item.DataBlockName.Equals(chName))
                    {
                        result = item;
                        break;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        ///     Hàm đọc danh sách các gói dữ liệu.
        /// </summary>
        /// <param name="dvNode">XmlNode</param>
        /// <returns>Danh sách gói dữ liệu</returns>
        public List<DataBlock> GetDataBlocks(XmlNode dvNode)
        {
            var dbList = new List<DataBlock>();
            try
            {
                foreach (XmlNode dbNote in dvNode)
                {
                    var db = new DataBlock();
                    db.ChannelId = int.Parse(dbNote.Attributes[CHANNEL_ID].Value);
                    db.DeviceId = int.Parse(dbNote.Attributes[DEVICE_ID].Value);
                    db.DataBlockId = int.Parse(dbNote.Attributes[DATABLOCK_ID].Value);
                    db.DataBlockName = dbNote.Attributes[DATABLOCK_NAME].Value;
                    db.TypeOfRead = $"{dbNote.Attributes[TypeOfRead].Value}";
                    db.StartAddress = ushort.Parse(dbNote.Attributes[START_ADDRESS].Value);
                    db.MemoryType = $"{dbNote.Attributes[MemoryType].Value}";
                    db.Length = ushort.Parse(dbNote.Attributes[LENGTH].Value);
                    db.DataType = dbNote.Attributes[DATA_TYPE].Value;
                    db.Description = dbNote.Attributes[ChannelManager.DESCRIPTION].Value;
                    db.Tags = objTagManager.GetTags(dbNote);
                    dbList.Add(db);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dbList;
        }
    }
}
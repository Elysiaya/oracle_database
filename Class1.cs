
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 废案
/// </summary>
namespace 空间数据库
{
    public class SdoPoint : IOracleCustomType, INullable
    {
        [OracleObjectMapping(0)]
        public float X;
        [OracleObjectMapping(1)]
        public float Y;
        [OracleObjectMapping(2)]
        public float Z;

        private bool m_bIsNull;

        public bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }

        public static SdoPoint Null
        {
            get
            {
                SdoPoint obj = new SdoPoint();
                obj.m_bIsNull = true;
                return obj;
            }
        }
        public override string ToString()
        {
            if (m_bIsNull)
                return "SdoPoint.Null";
            else
                return "SdoPoint(" + X + "," + Y + "," + Z + ")";
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            // If the UDT may contain NULL attribute data, enable the following code
            //if (!OracleUdt.IsDBNull(con, pUdt, 0))
            X = (float)OracleUdt.GetValue(con, pUdt, 0);

            // If the UDT may contain NULL attribute data, enable the following code
            //if (!OracleUdt.IsDBNull(con, pUdt, 1))
            Y = (float)OracleUdt.GetValue(con, pUdt, 1);

            // If the UDT may contain NULL attribute data, enable the following code
            //if (!OracleUdt.IsDBNull(con, pUdt, 2))
            Z = (float)OracleUdt.GetValue(con, pUdt, 2);
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, X);
            OracleUdt.SetValue(con, pUdt, 1, Y);
            OracleUdt.SetValue(con, pUdt, 2, Z);
        }
    }

    [OracleCustomTypeMapping("MDSYS.SDO_POINT_TYPE")]
    public class SdoPointFactory : IOracleCustomTypeFactory
    {
        // IOracleCustomTypeFactory Inteface
        public IOracleCustomType CreateObject()
        {
            SdoPoint sdoPoint = new SdoPoint();
            return sdoPoint;
        }
    }
    public class SdoGeometry : INullable, IOracleCustomType
    {
        [OracleObjectMapping(0)]
        public int _gtype;
        [OracleObjectMapping(1)]
        public int _srid;
        [OracleObjectMapping(2)]
        public SdoPoint _point;
        [OracleObjectMapping(3)]
        public int[] _elementInfo;
        [OracleObjectMapping(4)]
        public float[] _ordinates;

        private bool m_bIsNull;

        public bool IsNull
        {
            get
            {
                return m_bIsNull;
            }
        }

        public static SdoGeometry Null
        {
            get
            {
                SdoGeometry obj = new SdoGeometry();
                obj.m_bIsNull = true;
                return obj;
            }
        }
        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            // If the UDT may contain NULL attribute data, enable the following code
            //if (!OracleUdt.IsDBNull(con, pUdt, 0))
            _gtype = (int)OracleUdt.GetValue(con, pUdt, 0);

            // If the UDT may contain NULL attribute data, enable the following code
            //if (!OracleUdt.IsDBNull(con, pUdt, 0))
            if (OracleUdt.IsDBNull(con, pUdt, 1))
                _srid = 0;
            else
                _srid = (int)OracleUdt.GetValue(con, pUdt, 1);
            _point = (SdoPoint)OracleUdt.GetValue(con, pUdt, 2);
            _elementInfo = (int[])OracleUdt.GetValue(con, pUdt, 3);
            _ordinates = (float[])OracleUdt.GetValue(con, pUdt, 4);
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, _gtype);
            OracleUdt.SetValue(con, pUdt, 1, _srid);
            OracleUdt.SetValue(con, pUdt, 2, _point);
            OracleUdt.SetValue(con, pUdt, 3, _elementInfo);
            OracleUdt.SetValue(con, pUdt, 4, _ordinates);
        }

        public int[] ElementInfo
        {
            get
            {
                return _elementInfo;
            }
        }

        public float[] Ordinates
        {
            get
            {
                return _ordinates;
            }
        }

        public override string ToString()
        {
            string eleminfostr = String.Empty, ordinatesstr = String.Empty;
            if (m_bIsNull)
                return "SdoGeometry.Null";
            else
            {
                eleminfostr = _elementInfo[0].ToString();
                for (int i = 1; i < _elementInfo.Length; i++)
                    eleminfostr += "," + _elementInfo[i];
                eleminfostr = "ElementInfo(" + eleminfostr + ")";

                ordinatesstr = _ordinates[0].ToString();
                for (int i = 1; i < _ordinates.Length; i++)
                    ordinatesstr += "," + _ordinates[i];
                ordinatesstr = "Ordinates(" + ordinatesstr + ")";
            }
            return String.Format("SdoGeometry({0},{1},{2},{3},{4})",
              _gtype, _srid, _point, eleminfostr, ordinatesstr);
        }
    }

    [OracleCustomTypeMapping("MDSYS.SDO_GEOMETRY")]
    public class SdoGeometryFactory : IOracleCustomTypeFactory
    {
        // IOracleCustomTypeFactory Inteface
        public IOracleCustomType CreateObject()
        {
            return new SdoGeometry();
        }
    }

    [OracleCustomTypeMapping("MDSYS.SDO_ELEM_INFO_ARRAY")]
    public class SdoElemInfoArrayFactory : IOracleArrayTypeFactory
    {
        // IOracleArrayTypeFactory.CreateArray Inteface
        public Array CreateArray(int numElems)
        {
            return new int[numElems];
        }

        // IOracleArrayTypeFactory.CreateStatusArray
        public Array CreateStatusArray(int numElems)
        {
            // An OracleUdtStatus[] is not required to store null status information
            // if there is no NULL attribute data in the element array
            return null;
        }
    }

    [OracleCustomTypeMapping("MDSYS.SDO_ORDINATE_ARRAY")]
    public class SdoOrdinateArrayFactory : IOracleArrayTypeFactory
    {
        // IOracleArrayTypeFactory.CreateArray Inteface
        public Array CreateArray(int numElems)
        {
            return new float[numElems];
        }

        // IOracleArrayTypeFactory.CreateStatusArray
        public Array CreateStatusArray(int numElems)
        {
            // An OracleUdtStatus[] is not required to store null status information
            // if there is no NULL attribute data in the element array
            return null;

        }
    }

}

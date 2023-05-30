using glTFLoader;
using glTFLoader.Schema;
using Xbim.Common.Geometry;
using Xbim.Common.Step21;
using Xbim.GLTF;
using Xbim.Ifc;
using Xbim.ModelGeometry.Scene;

namespace conversionTest
{
    internal static class Ifc2GlbConverterBase
    {
        public static byte[] ConvertToGlb(byte[] ifcFile, double deflectionAngle = 100, double deflectionTolerance = 1000, XbimSchemaVersion ifcVersion = XbimSchemaVersion.Ifc2X3)
        {
            using MemoryStream ms = new(ifcFile);
            return ConvertToGlb(ms, deflectionAngle, deflectionTolerance, ifcVersion);
        }
        public static byte[] ConvertToGlb(Stream ifcFile, double deflectionAngle = 100, double deflectionTolerance = 1000, XbimSchemaVersion ifcVersion = XbimSchemaVersion.Ifc2X3)
        {
            byte[] glbFileBytes;
            Gltf gltf;
            Xbim3DModelContext context;
            Builder bldr;
            using (var model = IfcStore.Open(ifcFile, Xbim.IO.StorageType.Ifc, ifcVersion, Xbim.IO.XbimModelType.MemoryModel))
            {

                model.ModelFactors.ProfileDefLevelOfDetail = 0;
                model.ModelFactors.DeflectionAngle = deflectionAngle;
                model.ModelFactors.SimplifyFaceCountThreshHold = 50;
                model.ModelFactors.DeflectionTolerance = deflectionTolerance;
                model.ModelFactors.ShortestEdgeLength = 50;

                context = new Xbim3DModelContext(model);
                context.CreateContext();
                bldr = new Builder();
                bldr.BufferInBase64 = true;
                gltf = bldr.BuildInstancedScene(model, XbimMatrix3D.Identity);
            }

            using (MemoryStream ms2 = new MemoryStream())
            {
                gltf.SaveBinaryModel(null, ms2);
                glbFileBytes = ms2.ToArray();
            }

            return glbFileBytes;

        }
    }
}
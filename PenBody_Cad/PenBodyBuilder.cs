using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenBody_Cad
{
    public class PenBodyBuilder
    {
        public void Build(PenBody penBody)
        {
            var cadConnector = new CadConnector();
            cadConnector.Connect();

            var document = cadConnector.Kompas.Document3D();
            document.Create(false, true);
            document = (ksDocument3D)cadConnector.Kompas.ActiveDocument3D();

            var detail = (ksPart)document.GetPart((short)Part_Type.pTop_Part);
            var currentPlane = (ksEntity)detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);

            var entitySketch = (ksEntity)detail.NewEntity((short)Obj3dType.o3d_sketch);
            var sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
            sketchDefinition.SetPlane(currentPlane);
            entitySketch.Create();

            var sketchEdit = (ksDocument2D)sketchDefinition.BeginEdit();
            sketchEdit.ksLineSeg(penBody.InnerDiameter / 2, 0, (penBody.InnerDiameter / 2 + 1), 0, 1);
            sketchEdit.ksLineSeg((penBody.InnerDiameter / 2 + 1), 0, (penBody.InnerDiameter / 2 + 1), 5, 1);
            sketchEdit.ksLineSeg((penBody.InnerDiameter / 2 + 1), 5, (penBody.MainDiameter * 0.87 / 2), 5, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter * 0.87 / 2), 5, (penBody.MainDiameter * 0.87 / 2), penBody.MainLength + 5, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter * 0.87 / 2), penBody.MainLength + 5, (penBody.MainDiameter / 2), penBody.MainLength + 5, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + 5, (penBody.MainDiameter / 2), penBody.MainLength + 7, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + 7, (penBody.RubberDiameter / 2), penBody.MainLength + 7, 1);

            sketchEdit.ksLineSeg((penBody.RubberDiameter / 2), penBody.MainLength + 7, (penBody.RubberDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, 1);
            sketchEdit.ksLineSeg((penBody.RubberDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, (penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, (penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 10, 1);
            sketchEdit.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 10, (penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 10, 1);
            sketchEdit.ksLineSeg((penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 10, (penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 15, 1);
            sketchEdit.ksLineSeg((penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 15, penBody.InnerDiameter / 2, penBody.MainLength + penBody.RubberLength + 15, 1);
            sketchEdit.ksLineSeg(penBody.InnerDiameter / 2, penBody.MainLength + penBody.RubberLength + 15, penBody.InnerDiameter / 2, 0, 1);
            sketchEdit.ksLineSeg(0, 0, 0, penBody.MainLength + penBody.RubberLength + 15, 3);
            sketchDefinition.EndEdit();

            var entityRotated = (ksEntity)detail.NewEntity((short)Obj3dType.o3d_baseRotated);
            var entityRotatedDefinition = (ksBaseRotatedDefinition)entityRotated.GetDefinition();
            entityRotatedDefinition.directionType = 0;
            entityRotatedDefinition.SetSideParam(true, 360);
            entityRotatedDefinition.SetSketch(entitySketch);
            entityRotated.Create();

            var newPlane = (ksEntity)detail.NewEntity((short)Obj3dType.o3d_planeOffset);

            var newPlaneDefinition = (ksPlaneOffsetDefinition)newPlane.GetDefinition();
            newPlaneDefinition.SetPlane((ksEntity)detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ));
            newPlaneDefinition.direction = true;
            newPlaneDefinition.offset = 5;
            newPlane.Create();

            var newEntitySketch = (ksEntity)detail.NewEntity((short)Obj3dType.o3d_sketch);
            var sketchDefinition1 = (ksSketchDefinition)newEntitySketch.GetDefinition();
            sketchDefinition1.SetPlane(newPlane);
            newEntitySketch.Create();

            var poly = GetPolygon(cadConnector, penBody);

            sketchEdit = (ksDocument2D)sketchDefinition1.BeginEdit();
            sketchEdit.ksCircle(0, 0, penBody.InnerDiameter / 2, 1);
            sketchEdit.ksRegularPolygon(poly);
            sketchDefinition1.EndEdit();

            var entityExtrude1 = (ksEntity)detail.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            var entityExtrudeDefinition1 = (ksBaseExtrusionDefinition)entityExtrude1.GetDefinition();
            entityExtrudeDefinition1.SetSideParam(true, 0, penBody.MainLength);
            entityExtrudeDefinition1.SetSketch(newEntitySketch);
            entityExtrude1.Create();
        }

        private ksRegularPolygonParam GetPolygon(CadConnector cadConnector, PenBody penBody)
        {
            var poly = (ksRegularPolygonParam)cadConnector.Kompas.GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
            poly.ang = 0;
            poly.count = 6;
            poly.describe = true; // Вписанный многоугольник
            poly.radius = 0.87 * penBody.MainDiameter / 2;
            poly.style = 1; // Стиль линии - основной
            poly.xc = 0;
            poly.yc = 0;

            return poly;
        }
    }
}

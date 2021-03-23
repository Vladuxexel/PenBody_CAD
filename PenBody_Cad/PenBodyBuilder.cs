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
        private CadConnector _cadConnector;
        private ksDocument3D _document;
        private ksEntity _entitySketch;
        private ksPart _detail;
        private ksSketchDefinition _sketchDefinition;
        private ksEntity _currentPlane;
        private ksDocument2D _sketch2D;

        public void Build(PenBody penBody)
        {
            _cadConnector = new CadConnector();
            _cadConnector.Connect();
            _document = _cadConnector.Kompas.Document3D();
            _document.Create(false, true);
            _document = (ksDocument3D)_cadConnector.Kompas.ActiveDocument3D();
            _detail = (ksPart)_document.GetPart((short)Part_Type.pTop_Part);

            _currentPlane = (ksEntity)_detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            DrawBase(penBody);
            SpinExtrude();

            _currentPlane = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_planeOffset);
            DrawPolygon(penBody);
            Extrude(penBody);            
        }

        private void DrawBase(PenBody penBody)
        {
            CreateSketch();
            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketch2D.ksLineSeg(penBody.InnerDiameter / 2, 0, (penBody.InnerDiameter / 2 + 1), 0, 1);
            _sketch2D.ksLineSeg((penBody.InnerDiameter / 2 + 1), 0, (penBody.InnerDiameter / 2 + 1), 5, 1);
            _sketch2D.ksLineSeg((penBody.InnerDiameter / 2 + 1), 5, (penBody.MainDiameter * 0.87 / 2), 5, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter * 0.87 / 2), 5, (penBody.MainDiameter * 0.87 / 2), penBody.MainLength + 5, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter * 0.87 / 2), penBody.MainLength + 5, (penBody.MainDiameter / 2), penBody.MainLength + 5, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + 5, (penBody.MainDiameter / 2), penBody.MainLength + 7, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + 7, (penBody.RubberDiameter / 2), penBody.MainLength + 7, 1);
            _sketch2D.ksLineSeg((penBody.RubberDiameter / 2), penBody.MainLength + 7, (penBody.RubberDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, 1);
            _sketch2D.ksLineSeg((penBody.RubberDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, (penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 7, (penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 10, 1);
            _sketch2D.ksLineSeg((penBody.MainDiameter / 2), penBody.MainLength + penBody.RubberLength + 10, (penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 10, 1);
            _sketch2D.ksLineSeg((penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 10, (penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 15, 1);
            _sketch2D.ksLineSeg((penBody.InnerDiameter / 2 + 1), penBody.MainLength + penBody.RubberLength + 15, penBody.InnerDiameter / 2, penBody.MainLength + penBody.RubberLength + 15, 1);
            _sketch2D.ksLineSeg(penBody.InnerDiameter / 2, penBody.MainLength + penBody.RubberLength + 15, penBody.InnerDiameter / 2, 0, 1);
            _sketch2D.ksLineSeg(0, 0, 0, penBody.MainLength + penBody.RubberLength + 15, 3);
            _sketchDefinition.EndEdit();
        }

        private void CreateSketch()
        {
            _entitySketch = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_sketch);
            _sketchDefinition = (ksSketchDefinition)_entitySketch.GetDefinition();
            _sketchDefinition.SetPlane(_currentPlane);
            _entitySketch.Create();
        }

        private void SpinExtrude()
        {
            var entityRotated = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_baseRotated);
            var entityRotatedDefinition = (ksBaseRotatedDefinition)entityRotated.GetDefinition();
            entityRotatedDefinition.directionType = 0;
            entityRotatedDefinition.SetSideParam(true, 360);
            entityRotatedDefinition.SetSketch(_entitySketch);
            entityRotated.Create();
        }

        private ksRegularPolygonParam GetPolygon(PenBody penBody)
        {
            var poly = (ksRegularPolygonParam)_cadConnector.Kompas.GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
            poly.ang = 0;
            poly.count = 6;
            poly.describe = true; // Вписанный многоугольник
            poly.radius = 0.87 * penBody.MainDiameter / 2;
            poly.style = 1; // Стиль линии - основной
            poly.xc = 0;
            poly.yc = 0;

            return poly;
        }

        private void DrawPolygon(PenBody penBody)
        {
            CreateNewPlane();
            _entitySketch = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_sketch);
            _sketchDefinition = (ksSketchDefinition)_entitySketch.GetDefinition();
            _sketchDefinition.SetPlane(_currentPlane);
            _entitySketch.Create();

            var poly = GetPolygon(penBody);

            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketch2D.ksCircle(0, 0, penBody.InnerDiameter / 2, 1);
            _sketch2D.ksRegularPolygon(poly);
            _sketchDefinition.EndEdit();
        }

        private void CreateNewPlane()
        {
            var newPlaneDefinition = (ksPlaneOffsetDefinition)_currentPlane.GetDefinition();
            newPlaneDefinition.SetPlane((ksEntity)_detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ));
            newPlaneDefinition.direction = true;
            newPlaneDefinition.offset = 5;
            _currentPlane.Create();
        }

        private void Extrude(PenBody penBody)
        {
            var entityExtrude1 = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            var entityExtrudeDefinition1 = (ksBaseExtrusionDefinition)entityExtrude1.GetDefinition();
            entityExtrudeDefinition1.SetSideParam(true, 0, penBody.MainLength);
            entityExtrudeDefinition1.SetSketch(_entitySketch);
            entityExtrude1.Create();
        }
    }
}

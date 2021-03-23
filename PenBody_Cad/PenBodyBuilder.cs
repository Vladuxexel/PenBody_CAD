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
    /// <summary>
    /// Класс для построения детали по заданным параметрам.
    /// </summary>
    public class PenBodyBuilder
    {
        /// <summary>
        /// Соединение с Компас-3D.
        /// </summary>
        private CadConnector _cadConnector;

        /// <summary>
        /// Ссылка на документ.
        /// </summary>
        private ksDocument3D _document;

        /// <summary>
        /// Ссылка на эскиз.
        /// </summary>
        private ksEntity _entitySketch;

        /// <summary>
        /// Ссылка на деталь.
        /// </summary>
        private ksPart _detail;

        /// <summary>
        /// Ссылка на свойства эскиза.
        /// </summary>
        private ksSketchDefinition _sketchDefinition;

        /// <summary>
        /// Ссылка на текущую плоскость.
        /// </summary>
        private ksEntity _currentPlane;

        /// <summary>
        /// Ссылка на эскиз 2D.
        /// </summary>
        private ksDocument2D _sketch2D;

        /// <summary>
        /// Метод запуска процесса построения детали.
        /// </summary>
        /// <param name="penBodyParameters">Объект с параметрами.</param>
        public void Build(PenBodyParameters penBodyParameters)
        {
            _cadConnector = new CadConnector();
            _cadConnector.Connect();
            _document = _cadConnector.Kompas.Document3D();
            _document.Create(false, true);
            _document = (ksDocument3D)_cadConnector.Kompas.ActiveDocument3D();
            _detail = (ksPart)_document.GetPart((short)Part_Type.pTop_Part);

            _currentPlane = (ksEntity)_detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            DrawBase(penBodyParameters);
            SpinExtrude();

            _currentPlane = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_planeOffset);
            DrawPolygon(penBodyParameters);
            Extrude(penBodyParameters);            
        }

        /// <summary>
        /// Метод отрисовки 2d эскиза основы корпуса ручки для вращательного выдавливания.
        /// </summary>
        /// <param name="penBodyParameters"></param>
        private void DrawBase(PenBodyParameters penBodyParameters)
        {
            CreateSketch();
            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketch2D.ksLineSeg(penBodyParameters.InnerDiameter / 2, 0, (penBodyParameters.InnerDiameter / 2 + 1), 0, 1);
            _sketch2D.ksLineSeg((penBodyParameters.InnerDiameter / 2 + 1), 0, (penBodyParameters.InnerDiameter / 2 + 1), 5, 1);
            _sketch2D.ksLineSeg((penBodyParameters.InnerDiameter / 2 + 1), 5, (penBodyParameters.MainDiameter * 0.87 / 2), 5, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter * 0.87 / 2), 5, (penBodyParameters.MainDiameter * 0.87 / 2), penBodyParameters.MainLength + 5, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter * 0.87 / 2), penBodyParameters.MainLength + 5, (penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + 5, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + 5, (penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + 7, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + 7, (penBodyParameters.RubberDiameter / 2), penBodyParameters.MainLength + 7, 1);
            _sketch2D.ksLineSeg((penBodyParameters.RubberDiameter / 2), penBodyParameters.MainLength + 7, (penBodyParameters.RubberDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 7, 1);
            _sketch2D.ksLineSeg((penBodyParameters.RubberDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 7, (penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 7, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 7, (penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 10, 1);
            _sketch2D.ksLineSeg((penBodyParameters.MainDiameter / 2), penBodyParameters.MainLength + penBodyParameters.RubberLength + 10, (penBodyParameters.InnerDiameter / 2 + 1), penBodyParameters.MainLength + penBodyParameters.RubberLength + 10, 1);
            _sketch2D.ksLineSeg((penBodyParameters.InnerDiameter / 2 + 1), penBodyParameters.MainLength + penBodyParameters.RubberLength + 10, (penBodyParameters.InnerDiameter / 2 + 1), penBodyParameters.MainLength + penBodyParameters.RubberLength + 15, 1);
            _sketch2D.ksLineSeg((penBodyParameters.InnerDiameter / 2 + 1), penBodyParameters.MainLength + penBodyParameters.RubberLength + 15, penBodyParameters.InnerDiameter / 2, penBodyParameters.MainLength + penBodyParameters.RubberLength + 15, 1);
            _sketch2D.ksLineSeg(penBodyParameters.InnerDiameter / 2, penBodyParameters.MainLength + penBodyParameters.RubberLength + 15, penBodyParameters.InnerDiameter / 2, 0, 1);
            _sketch2D.ksLineSeg(0, 0, 0, penBodyParameters.MainLength + penBodyParameters.RubberLength + 15, 3);
            _sketchDefinition.EndEdit();
        }

        /// <summary>
        /// Метод создания эскиза.
        /// </summary>
        private void CreateSketch()
        {
            _entitySketch = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_sketch);
            _sketchDefinition = (ksSketchDefinition)_entitySketch.GetDefinition();
            _sketchDefinition.SetPlane(_currentPlane);
            _entitySketch.Create();
        }

        /// <summary>
        /// Метод выдавливания вращением.
        /// </summary>
        private void SpinExtrude()
        {
            var entityRotated = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_baseRotated);
            var entityRotatedDefinition = (ksBaseRotatedDefinition)entityRotated.GetDefinition();
            entityRotatedDefinition.directionType = 0;
            entityRotatedDefinition.SetSideParam(true, 360);
            entityRotatedDefinition.SetSketch(_entitySketch);
            entityRotated.Create();
        }

        /// <summary>
        /// Метод получения многоугольника.
        /// </summary>
        /// <param name="penBodyParameters">Объект с параметрами.</param>
        /// <returns>Полигон</returns>
        private ksRegularPolygonParam GetPolygon(PenBodyParameters penBodyParameters)
        {
            var poly = (ksRegularPolygonParam)_cadConnector.Kompas.GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
            poly.ang = 0;
            poly.count = 6;
            poly.describe = true; // Вписанный многоугольник
            poly.radius = 0.87 * penBodyParameters.MainDiameter / 2;
            poly.style = 1; // Стиль линии - основной
            poly.xc = 0;
            poly.yc = 0;

            return poly;
        }

        /// <summary>
        /// Метод отрисовки полого полигона.
        /// </summary>
        /// <param name="penBodyParameters">Объект с параметрами.</param>
        private void DrawPolygon(PenBodyParameters penBodyParameters)
        {
            CreateNewPlane();
            CreateSketch();

            var poly = GetPolygon(penBodyParameters);

            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketch2D.ksCircle(0, 0, penBodyParameters.InnerDiameter / 2, 1);
            _sketch2D.ksRegularPolygon(poly);
            _sketchDefinition.EndEdit();
        }

        /// <summary>
        /// Метод создания плоскости.
        /// </summary>
        private void CreateNewPlane()
        {
            var newPlaneDefinition = (ksPlaneOffsetDefinition)_currentPlane.GetDefinition();
            newPlaneDefinition.SetPlane((ksEntity)_detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ));
            newPlaneDefinition.direction = true;
            newPlaneDefinition.offset = 5;
            _currentPlane.Create();
        }

        /// <summary>
        /// Метод выдавливанияю
        /// </summary>
        /// <param name="penBodyParameters">Объект с параметрами.</param>
        private void Extrude(PenBodyParameters penBodyParameters)
        {
            var entityExtrude1 = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            var entityExtrudeDefinition1 = (ksBaseExtrusionDefinition)entityExtrude1.GetDefinition();
            entityExtrudeDefinition1.SetSideParam(true, 0, penBodyParameters.MainLength);
            entityExtrudeDefinition1.SetSketch(_entitySketch);
            entityExtrude1.Create();
        }
    }
}

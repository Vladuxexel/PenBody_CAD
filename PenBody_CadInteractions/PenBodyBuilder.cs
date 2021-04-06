using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using PenBody_Cad.Enums;
using System.Windows;

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
        /// Параметры модели.
        /// </summary>
        private PenBodyParametersList _penBodyParametersList;

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
        /// <param name="penBodyParametersList">Объект с параметрами.</param>
        public void Build(PenBodyParametersList penBodyParametersList)
        {
            InitReferences(penBodyParametersList);

            //Создание основы корпуса
            _currentPlane = (ksEntity)_detail.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            DrawBase();
            SpinExtrude();

            //Создание формы основной части
            _currentPlane = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_planeOffset);
            DrawPolygon();
            Extrude();
        }

        /// <summary>
        /// Метод отрисовки 2d эскиза основы корпуса ручки для вращательного выдавливания.
        /// </summary>
        private void DrawBase()
        {
            var mainLength = _penBodyParametersList[ParamName.MainLength];
            var rubberLength = _penBodyParametersList[ParamName.RubberLength];
            var mainRadius = _penBodyParametersList[ParamName.MainDiameter] / 2;
            var rubberRadius = _penBodyParametersList[ParamName.RubberDiameter] / 2;
            var innerRadius = _penBodyParametersList[ParamName.InnerDiameter] / 2;
            //Толщина части для резьбы
            var carvingWidth = 1;
            //Высота части для резьбы
            var carvingHeight = 5;
            //Высота круга перед резинкой
            var preRubberHeight = 2;
            //Высота круга после резинки
            var afterRubberHeight = 3;
            //Начало координат
            var baseXY = 0;
            //Тип основной линии
            var mainLine = 1;
            //Тип осевой линии
            var centerLine = 3;

            #region Точки эскиза
            var p0 = new Point(baseXY, baseXY);
            var p1 = new Point(innerRadius, baseXY);
            var p2 = new Point(innerRadius + carvingWidth, baseXY);
            var p3 = new Point(innerRadius + carvingWidth, mainLength + carvingHeight);
            var p4 = new Point(mainRadius, mainLength + carvingHeight);
            var p5 = new Point(mainRadius, mainLength + carvingHeight + preRubberHeight);
            var p6 = new Point(rubberRadius, mainLength + carvingHeight + preRubberHeight);
            var p7 = new Point(rubberRadius, mainLength + rubberLength + carvingHeight + preRubberHeight);
            var p8 = new Point(mainRadius, mainLength + rubberLength + carvingHeight + preRubberHeight);
            var p9 = new Point(mainRadius, 
                mainLength + rubberLength + carvingHeight + preRubberHeight + afterRubberHeight);
            var p10 = new Point(innerRadius + carvingWidth, 
                mainLength + rubberLength + carvingHeight + preRubberHeight + afterRubberHeight);
            var p11 = new Point(innerRadius + carvingWidth, 
                mainLength + rubberLength + carvingHeight * 2 + preRubberHeight + afterRubberHeight);
            var p12 = new Point(innerRadius, 
                mainLength + rubberLength + carvingHeight * 2 + preRubberHeight + afterRubberHeight);
            var p13 = new Point(baseXY, 
                mainLength + rubberLength + carvingHeight * 2 + preRubberHeight + afterRubberHeight);
            #endregion

            CreateSketch();
            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            DrawLine(p1, p2, mainLine);
            DrawLine(p2, p3, mainLine);
            DrawLine(p3, p4, mainLine);
            DrawLine(p4, p5, mainLine);
            DrawLine(p5, p6, mainLine);
            DrawLine(p6, p7, mainLine);
            DrawLine(p7, p8, mainLine);
            DrawLine(p8, p9, mainLine);
            DrawLine(p9, p10, mainLine);
            DrawLine(p10, p11, mainLine);
            DrawLine(p11, p12, mainLine);
            DrawLine(p12, p1, mainLine);
            DrawLine(p0, p13, centerLine);
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
        /// <returns>Полигон</returns>
        private ksRegularPolygonParam GetPolygon()
        {
            var poly = (ksRegularPolygonParam)_cadConnector.Kompas
                .GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
            poly.ang = 0;
            poly.count = 6;
            poly.describe = true;
            poly.radius = 0.87 * _penBodyParametersList[ParamName.MainDiameter] / 2;
            poly.style = 1;
            poly.xc = 0;
            poly.yc = 0;

            return poly;
        }

        /// <summary>
        /// Метод отрисовки полого полигона.
        /// </summary>
        private void DrawPolygon()
        {
            CreateNewPlane();
            CreateSketch();

            var poly = GetPolygon();

            _sketch2D = (ksDocument2D)_sketchDefinition.BeginEdit();
            _sketch2D.ksCircle(0, 0, _penBodyParametersList[ParamName.InnerDiameter] / 2, 1);
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
        /// Метод выдавливания.
        /// </summary>
        private void Extrude()
        {
            var entityExtrude = (ksEntity)_detail.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            var entityExtrudeDefinition = (ksBaseExtrusionDefinition)entityExtrude.GetDefinition();
            entityExtrudeDefinition.SetSideParam(true, 0, _penBodyParametersList[ParamName.MainLength]);
            entityExtrudeDefinition.SetSketch(_entitySketch);
            entityExtrude.Create();
        }

        /// <summary>
        /// Метод отрисовки линии на 2D области.
        /// </summary>
        /// <param name="startPoint">Точка начала линии.</param>
        /// <param name="endPoint">Точка конца линии.</param>
        /// <param name="lineType">Тип линии.</param>
        private void DrawLine(Point startPoint, Point endPoint, int lineType) =>
            _sketch2D.ksLineSeg(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, lineType);

        /// <summary>
        /// Инициализация ссылок для работы с Компасом.
        /// </summary>
        /// <param name="penBodyParametersList">Модель параметров.</param>
        private void InitReferences(PenBodyParametersList penBodyParametersList)
        {
            _penBodyParametersList = penBodyParametersList;
            _cadConnector = new CadConnector();
            _cadConnector.Connect();
            _document = _cadConnector.Kompas.Document3D();
            _document.Create(false, true);
            _document = (ksDocument3D)_cadConnector.Kompas.ActiveDocument3D();
            _detail = (ksPart)_document.GetPart((short)Part_Type.pTop_Part);
        }
    }
}

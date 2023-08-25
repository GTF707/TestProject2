using Domain;
using DTO;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NpgsqlTypes;
using Repository.Interface;

namespace TestProject2.Controllers
{
    public class ShapeController : Controller
    {
        private readonly IRepository<Shape> shapeRepository;
        private readonly IRepository<ShapeData> dataRepository;
        public ShapeController(IRepository<Shape> shaperepository, IRepository<ShapeData> datarepository)
        {
            shapeRepository = shaperepository;
            dataRepository = datarepository;

        }
        // GET: ShapeController
        public ActionResult Index()
        {

            return View();
        }

        // GET: ShapeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShapeController/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShapeDTO cordinate)
        {



            FeatureCollection obj = JsonConvert.DeserializeObject<FeatureCollection>(cordinate.Feature.ToString());

            var newData = new ShapeData()
            {
                Id = Guid.NewGuid(),
                Director = cordinate.Director,
                Address = cordinate.Address,
                TypeOfActivity = cordinate.TypeActivity,
            };
            dataRepository.Create(newData);
            List<Shape> shapes = new List<Shape>();
            foreach (var shape in obj.Features)
            {
                var newShape = new Shape()
                {
                    Id = Guid.NewGuid(),
                    DataId = newData.Id,
                    Figure = shape
                };
                shapes.Add(newShape);
            }
            await shapeRepository.CreateListAsync(shapes);

            return Ok();
        }

        //CheckShapeToPoint
        [HttpGet]
        public IActionResult CheckShape(Feature point)
        {
            var dataId = shapeRepository.CheckShape(point);
            var data = dataRepository.GetAll().Where(x => x.Id == dataId)
                .FirstOrDefault();

            return Ok(data);
        }

        //// POST: ShapeController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ShapeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShapeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShapeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShapeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

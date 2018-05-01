using System.Threading.Tasks;
using System.Web.Mvc;
using PGRating.Utils;
namespace PGRating.Controllers
{
    public class RatingTableController : Controller
    {
        // GET: RatingTable
        public async Task<ActionResult> Index()
        {
                var participantsViewModel = await ViewModelHelper.GetRatingCombinationTableAsync();

                return View(participantsViewModel);
            }
        }

        //// GET: RatingTable/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        RatingTableViewModel ratingTableViewModel = await db.RatingTableViewModels.FindAsync(id);
        //        if (ratingTableViewModel == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(ratingTableViewModel);
        //    }
        //}

        //// GET: RatingTable/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: RatingTable/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,Date,Data")] RatingTableViewModel ratingTableViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RatingTableViewModels.Add(ratingTableViewModel);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(ratingTableViewModel);
        //}

        //// GET: RatingTable/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RatingTableViewModel ratingTableViewModel = await db.RatingTableViewModels.FindAsync(id);
        //    if (ratingTableViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ratingTableViewModel);
        //}

        //// POST: RatingTable/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Data")] RatingTableViewModel ratingTableViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ratingTableViewModel).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(ratingTableViewModel);
        //}

        //// GET: RatingTable/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RatingTableViewModel ratingTableViewModel = await db.RatingTableViewModels.FindAsync(id);
        //    if (ratingTableViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ratingTableViewModel);
        //}

        //// POST: RatingTable/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    RatingTableViewModel ratingTableViewModel = await db.RatingTableViewModels.FindAsync(id);
        //    db.RatingTableViewModels.Remove(ratingTableViewModel);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    //}
}

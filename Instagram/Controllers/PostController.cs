using Instagram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Controllers
{
    public class PostController : Controller
    {
        // GET: PostController
        public ActionResult Dashboard()
        {
            var feed = new FeedViewModel
            {
                stories = new List<Story>
                {
                    new Story {
                        user=new User{
                            username = "aliaabhatt",
                            profile = "/images/nature.jpeg"
                        },
                        //media = new List<string>{ "/images/butterfly.jpeg" },
                        createdAt = DateTime.Now.AddHours(-2)
                    },
                    new Story {
                        user=new User{
                            username = "kritisanon",
                            profile = "/images/butterfly.jpeg",
                        },
                        //media = new List<string>{ "/images/flower.jpeg" },
                        createdAt = DateTime.Now.AddHours(-5)
                    },
                    new Story {
                        user=new User{
                            username = "narendramodi",
                            profile = "/images/flower.jpeg",
                        },
                        //media = new List<string>{ "/images/nature.jpeg" },
                        createdAt = DateTime.Now.AddHours(-8)
                    }
                },
                posts = new List<Post>
                {
                    new Post {
                        user=new User{
                            username = "netflix_in",
                            profile = "/images/flower.jpeg",
                        },
                        //media = "/images/nature.jpeg",
                        caption = "🔥 New thriller out now",
                        //location = "Mumbai, India",
                        //tagPeople = "srk, aliaabhatt",
                        likeCnt = 4521,
                        commentCnt = 210,
                        timeAgo = "1w",
                        createdAt = DateTime.Now.AddDays(-7)
                    },
                    new Post {
                        user=new User{
                            username = "marvel",
                            profile = "/images/butterfly.jpeg",
                        },
                        //media = "/images/nature.jpeg",
                        caption = "Are you ready for the multiverse?",
                        //location = "Los Angeles, CA",
                        likeCnt = 73210,
                        commentCnt = 1452,
                        timeAgo = "2d",
                        createdAt = DateTime.Now.AddDays(-2)
                    }
                }
            };

            return View();
        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
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

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobTriggerIsolatedN
{
    public class ToDoStore
    {
        private List<ToDo> _toDoList = new List<ToDo>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Learn Azure",
                IsFinished= false,
            }
        };

        public List<ToDo> GetToDos() => _toDoList;

        public void AddToDo(string toDo)
        {
            _toDoList.Add(new ToDo()
            {
                Id = Guid.NewGuid(),
                Description= toDo,
                IsFinished= false,
            });
        }

        public void MarkCompleted(Guid id)
        {
            var toDo = _toDoList.Find(x => x.Id == id);
            toDo.IsFinished = true;
        }
    }
}

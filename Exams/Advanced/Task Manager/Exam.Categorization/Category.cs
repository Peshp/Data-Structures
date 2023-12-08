using System;
using System.Collections.Generic;

namespace Exam.Categorization
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public void AddChild(Category child)
        {
            if (Children.Any(c => c.Id == child.Id))
                throw new ArgumentException("Child category already exists.");

            child.Parent = this;
            Children.Add(child);
        }

        public Category(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Children = new List<Category>();
        }
    }
}

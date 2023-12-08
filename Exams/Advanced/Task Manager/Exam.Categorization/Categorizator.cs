using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Categorization
{
    public class Categorizator : ICategorizator
    {
        private HashSet<Category> categories = new HashSet<Category>();

        public void AddCategory(Category category)
        {
            if (Contains(category))
                throw new ArgumentException("Category already exists.");

            categories.Add(category);
        }

        public void AssignParent(string childCategoryId, string parentCategoryId)
        {
            var child = GetCategoryById(childCategoryId);
            var parent = GetCategoryById(parentCategoryId);

            if (child.Parent != null)
                throw new ArgumentException("Child category already has a parent.");

            parent.AddChild(child);
        }

        public void RemoveCategory(string categoryId)
        {
            var categoryToRemove = GetCategoryById(categoryId);
            categories.Remove(categoryToRemove);
        }

        public bool Contains(Category category)
        {
            return categories.Contains(category);
        }

        public int Size()
        {
            return categories.Count;
        }

        public IEnumerable<Category> GetChildren(string categoryId)
        {
            var category = GetCategoryById(categoryId);
            return category.Children;
        }

        public IEnumerable<Category> GetHierarchy(string categoryId)
        {
            var category = GetCategoryById(categoryId);
            var hierarchy = new List<Category>();

            while (category != null)
            {
                hierarchy.Insert(0, category);
                category = category.Parent;
            }

            return hierarchy;
        }

        public IEnumerable<Category> GetTop3CategoriesOrderedByDepthOfChildrenThenByName()
        {
            return categories.OrderByDescending(c => GetDepthOfChildren(c))
                             .ThenBy(c => c.Name)
                             .Take(3);
        }

        private int GetDepthOfChildren(Category category)
        {
            if (category.Children.Count == 0)
                return 0;

            return 1 + category.Children.Max(GetDepthOfChildren);
        }

        private Category GetCategoryById(string categoryId)
        {
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                throw new ArgumentException("Category not found.");

            return category;
        }
    }
}
}

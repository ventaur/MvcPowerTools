﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CavemanTools.Model;
using FluentValidation;
using FluentValidation.Attributes;

namespace HtmlConventionsSample.Models
{
    public class MyModel
    {
        [Range(1,45)]
        public int Number { get; set; }
        public Guid Id { get; set; }
        
        [StringLength(45,MinimumLength = 2)]
        [Required]
        [RegularExpression(@"[\w]+")]
        public string Name { get; set; }
        
        public bool IsTrue { get; set; }
        public string[] Items { get; set; }
        public IdName Model { get; set; }

        public MyModel()
        {
            
        }
    }

    public class MyModelValidator : AbstractValidator<FluentMyModel>
    {
        public MyModelValidator()
        {
            RuleFor(d => d.Name).Length(2, 45).WithMessage("wrong name length");
        }
    }

    [Validator(typeof(MyModelValidator))]
    public class FluentMyModel
    {
        private string _selectedItem;

        public int Number { get; set; }
        public Guid Id { get; set; }
        
        
        public string Name { get; set; }
        
        public bool IsTrue { get; set; }
        public string[] Items { get; set; }

        public string SelectedItem
        {
            get
            {
                if (_selectedItem == null)
                {
                    return Items.Skip(1).FirstOrDefault();
                }
                return _selectedItem;
            }
            set { _selectedItem = value; }
        }


        public IdName Model { get; set; }

    }
}
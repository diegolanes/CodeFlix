﻿using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            (category.IsActive).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            (category.IsActive).Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");          
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action = () => new DomainEntity.Category("Category Name", null!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Description should not be empty or null");      
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("aa")]
        public void InstantiateErrorWhenNameIsLessThen3Characters(string invalidName)
        {
            Action action = () => new DomainEntity.Category(invalidName, "Category Ok description");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be at least 3 characters long");           
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsRatherThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsRatherThen255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, "Category Ok description");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsRatherThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsRatherThen10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category("Category Name", invalidDescription);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Description should be less or equal 10.000 characters long");        
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);

            category.Activate();

            //Assert
            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);

            category.Deactivate();

            //Assert
            category.IsActive.Should().BeFalse();            
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "new Name", Description = "new Description" };

            category.Update(newValues.Name, newValues.Description);

            category.Name.Should().Be(newValues.Name);
            category.Description.Should().Be(newValues.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newName = "new Name";
            category.Update(newName, category.Description);
            var currentDescription = category.Description;

            category.Name.Should().Be(newName);
            category.Description.Should().Be(currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");

            Action action = () => category.Update(name!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");            
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("aa")]
        public void UpdateErrorWhenNameIsLessThen3Characters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");

            Action action = () => category.Update(invalidName!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be at least 3 characters long");   
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsRatherThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsRatherThen255Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => category.Update(invalidName, "Category Ok description");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsRatherThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsRatherThen10_000Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => category.Update("Category Name", invalidDescription);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Description should be less or equal 10.000 characters long");
        }
    }
}

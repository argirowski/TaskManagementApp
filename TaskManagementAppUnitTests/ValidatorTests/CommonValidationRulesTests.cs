using Application.Validators;
using FluentAssertions;
using FluentValidation;

namespace TaskManagementAppUnitTests.ValidatorTests
{
    public class CommonValidationRulesTests
    {
        [Fact]
        public void ValidProjectId_ShouldPass()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidProjectId();
            var result = validator.Validate(Guid.NewGuid());
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidProjectId_EmptyOrDefault_ShouldFail()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidProjectId();
            validator.Validate(Guid.Empty).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidTaskId_ShouldPass()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidTaskId();
            var result = validator.Validate(Guid.NewGuid());
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidTaskId_EmptyOrDefault_ShouldFail()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidTaskId();
            validator.Validate(Guid.Empty).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidUserId_ShouldPass()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidUserId();
            var result = validator.Validate(Guid.NewGuid());
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidUserId_EmptyOrDefault_ShouldFail()
        {
            var validator = new InlineValidator<Guid>();
            validator.RuleFor(x => x).ValidUserId();
            validator.Validate(Guid.Empty).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidProjectName_Valid_ShouldPass()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidProjectName();
            validator.Validate("Valid Project").IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidProjectName_Invalid_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidProjectName();
            var invalidNames = new[] { null, "", "AB", "A" };
            foreach (var name in invalidNames)
            {
                validator.Validate(name ?? string.Empty).IsValid.Should().BeFalse();
            }
        }

        [Fact]
        public void ValidProjectName_TooLong_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidProjectName();
            var longName = new string('A', 51);
            validator.Validate(longName).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidTaskTitle_Valid_ShouldPass()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidTaskTitle();
            validator.Validate("Valid Task Title").IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidTaskTitle_Invalid_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidTaskTitle();
            var invalidTitles = new[] { null, "", "AB", "A" };
            foreach (var title in invalidTitles)
            {
                validator.Validate(title ?? string.Empty).IsValid.Should().BeFalse();
            }
        }

        [Fact]
        public void ValidTaskTitle_TooLong_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).ValidTaskTitle();
            var longTitle = new string('A', 101);
            validator.Validate(longTitle).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidTaskData_Valid_ShouldPass()
        {
            var validator = new InlineValidator<object>();
            validator.RuleFor(x => x).ValidTaskData();
            validator.Validate(new object()).IsValid.Should().BeTrue();
        }

        [Fact]
        public void OptionalDescription_Valid_ShouldPass()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).OptionalDescription(10, 500);
            validator.Validate("This is a valid description.").IsValid.Should().BeTrue();
        }

        [Fact]
        public void OptionalDescription_TooShortOrNull_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).OptionalDescription(10, 500);
            var invalidDescs = new[] { null, "", "short", "123456789" };
            foreach (var desc in invalidDescs)
            {
                validator.Validate(desc ?? string.Empty).IsValid.Should().BeFalse();
            }
        }

        [Fact]
        public void OptionalDescription_TooLong_ShouldFail()
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x).OptionalDescription(10, 500);
            var longDesc = new string('A', 501);
            validator.Validate(longDesc).IsValid.Should().BeFalse();
        }
    }
}

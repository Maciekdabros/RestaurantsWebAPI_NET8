using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Fast Food", "Traditional", "Vegetarian", "Vegan", "Italian", "Mexican", "Asian", "American", "Other", "Japanese", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);
        /*        RuleFor(dto => dto.Description)
                    .NotEmpty().WithMessage("Description is required");
                RuleFor(dto => dto.Category)
                    .NotEmpty().WithMessage("Insert a valid category");*/
        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Insert a valid email");
        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX)");
        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Insert a valid category");
        /*            .Custom((value, context) =>
                    {
                        var isValidCategory = validCategories.Contains(value);
                        if (!isValidCategory)
                        {
                            context.AddFailure("Category", "Insert a valid category");
                        }
                    });*/
    }
}
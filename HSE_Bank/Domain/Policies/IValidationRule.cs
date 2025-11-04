namespace HSE_Bank.Domain.Policies;

public interface IValidationRule<in T>
{
    void Check(T draft);
}
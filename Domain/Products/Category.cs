using Flunt.Validations;

namespace ProdutosApp.Domain.Products;

public class Category : Entity
{

    public string Name { get; private set; }
    public bool Active { get; private set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();

    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name", "O campo 'name' é obrigatório.") //validacao para o campo não vir nulo
            .IsGreaterOrEqualsThan(Name, 3, "Name", "O campo 'name' precisa ter mais que 3 caracteres.")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "O campo 'createdBy' é obrigatório.")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "O campo 'editedBy' é obrigatório.");
        AddNotifications(contract); //Valida o contrato e adiciona nas notificacoes
    }

    public void EditInfo(string name,bool active)
    {
        Active = active;
        Name = name;
        Validate();

    }
}

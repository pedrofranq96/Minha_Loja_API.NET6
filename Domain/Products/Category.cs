using Flunt.Validations;

namespace ProdutosApp.Domain.Products;

public class Category : Entity
{

    public string Name { get; set; }
    public bool Active { get; set; }

    public Category(string name, string createdBy, string editedBy)
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "Name", "O campo 'name' é obrigatório.") //validacao para o campo não vir nulo
            .IsGreaterOrEqualsThan(name,3,"Name","O campo 'name' precisa ter mais que 3 caracteres.")
            .IsNotNullOrEmpty(createdBy, "CreatedBy", "O campo 'createdBy' é obrigatório.") 
            .IsNotNullOrEmpty(editedBy, "EditedBy", "O campo 'editedBy' é obrigatório.");
        AddNotifications(contract); //Valida o contrato e adiciona nas notificacoes

        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
    }
}

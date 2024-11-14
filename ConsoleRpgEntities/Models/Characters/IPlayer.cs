using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Equipments;

namespace ConsoleRpgEntities.Models.Characters;

public interface IPlayer
{
    int Id { get; set; }
    string Name { get; set; }
    int Health { get; set; }
    int? EquipmentId { get; set; }

    ICollection<Ability> Abilities { get; set; }

    void Attack(ITargetable target);
    void UseAbility(IAbility ability, ITargetable target);
    void ListInventory();
    void AddItem(Item item);
    void RemoveItem(Item item);
    void UseItem(Item item, Equipment equipment);

}

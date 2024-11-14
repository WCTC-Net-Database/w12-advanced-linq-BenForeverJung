using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using ConsoleRpgEntities.Models.Equipments;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsoleRpgEntities.Models.Characters
{
    public class Player : ITargetable, IPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }

        // Foreign key
        public int? EquipmentId { get; set; }
        // public int? InventoryId { get; set; }

        //  Navigation properties
         public virtual Inventory Inventory { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual ICollection<Ability> Abilities { get; set; }
        //public virtual ICollection<Item>? InventoryItems { get; set; }


        public void Attack(ITargetable target)
        {
            // Player-specific attack logic
            Console.WriteLine($"{Name} attacks {target.Name} with a {Equipment.Weapon.Name} dealing {Equipment.Weapon.Attack} damage!");
            target.Health -= Equipment.Weapon.Attack;
            System.Console.WriteLine($"{target.Name} has {target.Health} health remaining.");

        }

        public void UseAbility(IAbility ability, ITargetable target)
        {
            if (Abilities.Contains(ability))
            {
                ability.Activate(this, target);
            }
            else
            {
                Console.WriteLine($"{Name} does not have the ability {ability.Name}!");
            }
        }

        //  Method to list all Items in the Players inventory

        public void ListInventory()
        {
            //var inventoryItems = this.Inventory.Items.Where()
            if (Inventory.Items.Count >= 1)
            {
                // TODO  add group by stat summary

                foreach (var item in Inventory.Items)
                {
                    Console.WriteLine(
                        $"#{item.Id} {item.Name}\tType: {item.Type}\t Attack Level: {item.Attack}\tDefense Level: {item.Defense}");

                }
            }
            else
            {
                Console.WriteLine("You do not have any Items in your Inventory.");
            }
        }

        //  Method to add an Item to the Players inventory
        public void AddItem(Item item)
        {
            if (item != null)
            {
                Inventory.Items.Add(item);
                Console.WriteLine($"{item.Name} has been added to your Inventory");
            }
            else
            {
                Console.WriteLine($"That is not a valid item from the item list to add to your Inventory.");
            }
        }


        //  Method to Remove an Item to the Players inventory
        public void RemoveItem(Item item)
        {
            if (Inventory.Items.Contains(item))
            {
                Inventory.Items.Remove(item);
                Console.WriteLine($"{item.Name} has been Removed from your Inventory");

            }
            else
            {
                Console.WriteLine($"You cannot remove {item.Name} because it is not in your Inventory.");
            }
        }

        //  Method to use or equip an Item from the Players inventory
        public void UseItem(Item item, Equipment equipment)
        {
            if (Inventory.Items.Contains(item))
            {

                if (item.Type == "Weapon")
                {
                    equipment.WeaponId = item.Id;
                    Console.WriteLine($"You have equipped your {item.Type} {item.Name}!");
                }
                else if (item.Type == "Armor")
                {
                    equipment.ArmorId = item.Id;
                    Console.WriteLine($"You have equipped your {item.Type} {item.Name}!");
                }
                else
                {
                    Console.WriteLine($"You used your {item.Type} {item.Name}!");
                }

                
            }
            else
            {
                Console.WriteLine($"You cannot use {item.Name} because it is not in your inventory.");
            }
        }

        // // Method to Equip Item - Covered by use Item Method
        //public void EquipItem(Item item)
        //{

        //}
    }
}

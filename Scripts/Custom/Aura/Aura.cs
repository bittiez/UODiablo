using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Bittiez.Aura
{
	public static class AURATYPE
	{
		public const int DISMOUNT_PLAYER = 1;
		public const int HEALING = 2;
		public const int DAMAGE = 3;
		public const int MANA_REGEN = 4;
		public const int LIFE_LEACH = 5;
		public const int STAT_BUFF = 6;
		public const int EFFECTAURA = 7;

		public const int CUSTOM = 9999;
	}

	public class Aura
	{
		#region SETTINGS
		private static TimeSpan AURAFREQUENCY = TimeSpan.FromSeconds(2); //How often will the aura "effect" trigger on a player
		private const int DEFAULT_HEAL_AMOUNT = 2; // Default amount of the healing aura
		private const int DEFAULT_DAMAGE_AMOUNT = 2; // Default damage amount for damaing auras
		private const int DEFAULT_STAT_BUFF = 3;
		#endregion

		private Timer m_InternalTimer;
		private readonly object m_Owner;
		private readonly int m_Distance;
		private readonly int m_AuraType;
		private TimeSpan m_OverideAuraFrequency = AURAFREQUENCY;

		public int HealingAmount = DEFAULT_HEAL_AMOUNT;
		public int DamageAmount = DEFAULT_DAMAGE_AMOUNT;
		public bool AffectsSelf = false;
		public int StatBuff = DEFAULT_STAT_BUFF;

		public TimeSpan OverideAuraFrequency
		{
			get { return m_OverideAuraFrequency; }
			set
			{
				m_OverideAuraFrequency = value;
				if (m_InternalTimer != null)
				{
					m_InternalTimer.Delay = value;
					m_InternalTimer.Interval = value;
				}
			}
		}

		public Aura(object owner, int distance, int auratype)
		{
			m_Owner = owner;
			m_Distance = distance;
			m_AuraType = auratype;

			m_InternalTimer = new AuraTimer(this);
		}

		public void Effect(Mobile target) //Triggered for each mobile inside the specified distance
		{
			if (m_AuraType == AURATYPE.CUSTOM)
			{
				//Custom aura code here
			}
			else
			{
				switch (m_AuraType)
				{
					case AURATYPE.DISMOUNT_PLAYER:
						if (target is PlayerMobile)
							((PlayerMobile)target).SetMountBlock(BlockMountType.Dazed, m_OverideAuraFrequency, true);
						break;
					case AURATYPE.HEALING:
						{
							Mobile auraOwner = AuraOwner();
							if (auraOwner != null)
								if (!auraOwner.CanBeBeneficial(target))
									break;
							target.Heal(HealingAmount);
							target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
						}
						break;
					case AURATYPE.DAMAGE:
						if (target.CanBeDamaged()) //Can't damage what can't be damaged
						{
							if (IsEnemy(target))
							{
								Mobile auraOwner = AuraOwner();
								if (auraOwner != null)
								{
									if (target.CanBeHarmedBy(auraOwner, false))
										target.Damage(DamageAmount, auraOwner);
								}
								else //Aura is not coming from a mobile.
									target.Damage(DamageAmount);
							}
						}
						break;
					case AURATYPE.MANA_REGEN:
						if (target.Mana < target.ManaMax)
						{
							if (m_Owner is Mobile)
							{
								if (((Mobile)m_Owner).CanBeBeneficial(target))
								{
									if (!IsEnemy(target))
										target.Mana += HealingAmount;
								}
							}
							else
								target.Mana += HealingAmount;
						}
						break;
					case AURATYPE.LIFE_LEACH:
						if (target.CanBeDamaged())
						{
							Mobile auraOwner = AuraOwner();
							if(auraOwner != null) //Aura is coming from a Mobile
							{
								if (IsEnemy(target))
								{
									target.Damage(DamageAmount, auraOwner);
									auraOwner.Heal(HealingAmount);
								}
							} else //Aura is coming from an item not a Mobile
							{
								//No mobile to heal, so this just acts as a damaging aura
								target.Damage(DamageAmount);
							}
						}
						break;
					case AURATYPE.STAT_BUFF:
						{
							Mobile auraOwner = AuraOwner();
							StatMod statMod = new StatMod(StatType.All, "Stat buff", StatBuff, m_OverideAuraFrequency);
							if (auraOwner != null) //Aura coming from a mobile
							{
								if (auraOwner.CanBeBeneficial(target))
								{
									target.AddStatMod(statMod);
								}
							}
							else //Aura coming from an item on the ground
							{
								target.AddStatMod(statMod);
							}
						}
						break;
					case AURATYPE.EFFECTAURA:
						target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
						break;
				}
			}
		}

		public void EnableAura()
		{
			m_InternalTimer.Start();
		}
		public void DisableAura()
		{
			m_InternalTimer.Stop();
		}
		public bool ToggleAura()
		{
			if (m_InternalTimer.Running) { m_InternalTimer.Stop(); return false; }
			else { m_InternalTimer.Start(); return true; }
		}

		/// <summary>
		/// Check if the target is considered an enemy of the aura owner. If the aura is on an item on the ground, every target is considered
		/// an enemy.
		/// </summary>
		/// <param name="target">Check if this target is an enemy.</param>
		/// <returns>true if the target is considered an enemy.</returns>
		private bool IsEnemy(Mobile target)
		{
			Mobile auraOwner = AuraOwner();

			if (auraOwner == null) //The aura is not on a mobile, so it should see everyone as an enemy
				return true;

			foreach (AggressorInfo aggressorInfo in (auraOwner).Aggressors)
			{
				if (aggressorInfo.Attacker == target) //This target is an aggressor/aggressed
					if (!aggressorInfo.Expired)
					{
						return true;
					}
			}
			foreach (AggressorInfo aggressorInfo in (auraOwner).Aggressed)
			{
				if (aggressorInfo.Defender == target) //This target is an aggressor/aggressed
					if (!aggressorInfo.Expired)
					{
						return true;
					}
			}
			return false;
		}
		/// <summary>
		/// Check if the aura is on a mobile, or an item a mobile is holding.
		/// </summary>
		/// <returns>Returns the mobile an aura is coming form, or null if it is not coming from a mobile.</returns>
		public Mobile AuraOwner()
		{
			if (m_Owner is Mobile)
				return m_Owner as Mobile;
			else if (m_Owner is Item) //Is the aura on an item?
			{
				Item itemOwner = (Item)m_Owner;
				if (itemOwner.Parent is Backpack) //Is the item in a backpack?
					if (((Backpack)itemOwner.Parent).Parent is Mobile) //Is the backpack on a mobile?
					{
						return ((Backpack)itemOwner.Parent).Parent as Mobile;
					}
				if (itemOwner.Parent is Mobile)
					return itemOwner.Parent as Mobile;
			}
			return null;
		}

		private class AuraTimer : Timer
		{
			private Aura m_Aura;
			public AuraTimer(Aura aura) : base(aura.OverideAuraFrequency, aura.OverideAuraFrequency)
			{
				m_Aura = aura;
			}

			protected override void OnTick()
			{

				if (m_Aura.m_Owner is Mobile)
				{
					foreach (Mobile m in ((Mobile)m_Aura.m_Owner).GetMobilesInRange(m_Aura.m_Distance))
					{
						if (!m_Aura.AffectsSelf && m_Aura.AuraOwner() == m)
							return;
						m_Aura.Effect(m);
					}
				}
				else if (m_Aura.m_Owner is Item)
				{
					foreach (Mobile m in ((Item)m_Aura.m_Owner).GetMobilesInRange(m_Aura.m_Distance))
					{
						if (!m_Aura.AffectsSelf && m_Aura.AuraOwner() == m)
							return;
						m_Aura.Effect(m);
					}
				}
			}
		}
	}
}

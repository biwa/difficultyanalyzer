#region ================== Copyright (c) 2015 Boris Iwanski

/*
 * Copyright (c) 2015, 2016 Boris Iwanski
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public struct AmmunitionType
	{
		public string name;
		public string title;
		public int dpu;

		public AmmunitionType(string name, string title, int dpu)
		{
			this.name = name;
			this.title = title;
			this.dpu = dpu;
		}
	}
}
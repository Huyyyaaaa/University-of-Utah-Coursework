<?php

use App\Models\Material;
use App\Models\Team;
use Illuminate\Database\Seeder;

class MaterialsTableSeeder extends Seeder
{
    public function run()
    {
        $team = Team::find(2);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Shingles',
            'description'   => 'Use only on slanted roofs.',
            'price'         => 40.10,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => '2" Nails',
            'description'   => 'Use for plywood',
            'price'         => 25.20,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => '4" Nails',
            'description'   => 'Use for concrete',
            'price'         => 30.30,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => '.5" Nails',
            'description'   => 'Use for shingles',
            'price'         => 15.40,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Caulk',
            'description'   => 'A waterproof filler and sealant, used in building work and repairs',
            'price'         => 5.50,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Sarnafil',
            'description'   => 'Use for commercial roofs',
            'price'         => 80.60,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Tar',
            'description'   => 'Use for commercial roofs',
            'price'         => 50.70,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Roof Glue',
            'description'   => 'Use for commercial roofs',
            'price'         => 10.80,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Rags',
            'description'   => 'Use to clean up',
            'price'         => 7.90,
            'created_by_id' => $team->users->random(),
        ]);
        factory(Material::class, 1)->create([
            'team_id'       => $team->id,
            'name'          => 'Plasty Tops',
            'description'   => 'Use to secure underlayment',
            'price'         => 30.00,
            'created_by_id' => $team->users->random(),
        ]);
    }
}

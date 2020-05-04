<?php

use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
    public function run()
    {
        $this->call([
            UsersTableSeeder::class,
            MaterialsTableSeeder::class,
            ProjectTemplatesTableSeeder::class,
            MaterialProjectTemplateTableSeeder::class,
            ProjectsTableSeeder::class,
            MaterialProjectTableSeeder::class,
            BidsTableSeeder::class,
            BidMaterialTableSeeder::class,
            TasksTableSeeder::class,
            ExtraChargesTableSeeder::class,
        ]);
    }
}

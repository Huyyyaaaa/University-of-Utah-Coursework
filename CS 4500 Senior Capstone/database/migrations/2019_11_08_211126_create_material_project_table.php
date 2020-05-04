<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateMaterialProjectTable extends Migration
{
    public function up()
    {
        Schema::create('material_project', function (Blueprint $table) {
            $table->unsignedBigInteger('material_id');
            $table->unsignedBigInteger('project_id');
            $table->unsignedBigInteger('quantity');

            $table->foreign('material_id')->references('id')->on('materials');
            $table->foreign('project_id')->references('id')->on('projects');
        });
    }

    public function down()
    {
        Schema::dropIfExists('material_project');
    }
}

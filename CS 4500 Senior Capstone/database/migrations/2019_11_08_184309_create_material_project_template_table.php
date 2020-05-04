<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateMaterialProjectTemplateTable extends Migration
{
    public function up()
    {
        Schema::create('material_project_template', function (Blueprint $table) {
            $table->unsignedBigInteger('material_id');
            $table->unsignedBigInteger('project_template_id');
            $table->unsignedBigInteger('quantity');

            $table->foreign('material_id')->references('id')->on('materials')->onDelete('cascade');
            $table->foreign('project_template_id')->references('id')->on('project_templates')->onDelete('cascade');
        });
    }

    public function down()
    {
        Schema::dropIfExists('material_project_template');
    }
}

<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreatePhotosTable extends Migration
{
    public function up()
    {
        Schema::create('photos', function (Blueprint $table) {
            $table->bigIncrements('id');
            $table->unsignedBigInteger('team_id');
            $table->unsignedBigInteger('project_id')->nullable();
            $table->unsignedBigInteger('material_id')->nullable();
            $table->string('url');
            $table->string('path');
            $table->string('token');
            $table->timestamps();

            $table->foreign('team_id')->references('id')->on('teams');
            $table->foreign('project_id')->references('id')->on('projects');
            $table->foreign('material_id')->references('id')->on('materials');
        });
    }

    public function down()
    {
        Schema::dropIfExists('photos');
    }
}

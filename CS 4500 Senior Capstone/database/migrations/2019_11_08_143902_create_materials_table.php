<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateMaterialsTable extends Migration
{
    public function up()
    {
        Schema::create('materials', function (Blueprint $table) {
            $table->bigIncrements('id');
            $table->unsignedBigInteger('team_id');
            $table->string('name');
            $table->text('description');
            $table->decimal('price');
            $table->bigInteger('sq_foot_coverage');
            $table->unsignedBigInteger('created_by_id');
            $table->timestamps();

            $table->foreign('team_id')->references('id')->on('teams');
            $table->foreign('created_by_id')->references('id')->on('users');
        });
    }

    public function down()
    {
        Schema::dropIfExists('materials');
    }
}
